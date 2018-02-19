Eon Wallet encryption proposal draft v.1.0
----------------------------------------

Id
--

Random Guid serialized as string

Name
----

Wallet name

version
-------

Wallet version. Must be 1 for current implementation.

keystore
--------
Object containing the encrypted seed and the necessary information to decrypt it if the correct key/password is provided.


***ciphertext***

    Encrypted seed


***mac***

    Message authentication code which is given as the SHA-256 of the concatenation of the last 16 bytes of the derived key together with the full ciphertext.
    SHA-256(DK[16..31] + ciphertext)


***crypto***

    Symetric encryption function to encrypt the seed using the result of DKF as the encryption key.
    
    cipher: aes-128-ctr
    iv: 128-bit initialisation vector for the cipher. lenght 16.
    
    **Note**: The key for the cipher is the leftmost 16 bytes of the derived key, i.e. DK[0..15]


***kdf***

    Key derivation function, current support for pbkdf2 hmac-sha384, future support for argon2d and argon2i

	pbkdf2
	------
	prf: "hmac-sha384",
	i: 131072. The number of iterations to perform to compute the hash.
	salt: random salt passed to pbkdf2, lenght 16.
		var salt = new byte[16];
		var Rng = System.Security.Cryptography.RandomNumberGenerator.Create();
		Rng.GetBytes(salt); 
	dklen: 32. length for the derived key. Must be >= 32.
	
	argon2d/argon2i
	---------------
	p: 1. degree of paralelism.
	msize: 8192. The amount of memory (in Kilobytes) to use to calculate the hash.
	i: 40. The number of iterations to perform to compute the hash. Because of Argon2's higher security, huge values like with PBKDF2 are not as necessary, although multiple iterations are still very much recommended.
	salt: random salt passed to argon2d/i. lenght 16.
        ```
        var salt = new byte[16];
        var Rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        Rng.GetBytes(salt); 
        ```
	adata: null. Additional associated data to use to compute the hash. This adds another layer of inderection for an attacker to reverse engineer the hash
	ksecret: null. An additional secret to use for the hash for extra security
	dklen: 32. length for the derived key. Must be >= 32.

    **Note**: Once the file's key has been derived (kdf), it should be verified through the derivation of the MAC

Json serialization description
------------------------------

```json
[
  {
    "id": "<GUID>",
    "name": "<wallet name>",
    "version": 1,

    "keystore":
    {
      "ciphertext": "<the encrypted seed, lenght 32>",
      "mac": "<message authentication code, lenght 32>",

      "crypto":
      {
        "cipher": "aes-128-ctr",
        "parameters":
        {
          "iv": "<initialization vector, lenght 16>"
        }
      },

      "kdf":
      {
        "function": "pbkdf2",
        "parameters":
        {
          "prf": "hmac-sha384",
          "i": "<The number of iterations to perform to compute the hash, defaults to 131072>",
          "salt": "<random salt passed to kdf, lenght 16>",
          "dklen": "<length for the derived key. Must be >= 32, defaults to 32>"
        }
      }  
    }
  },

  {
    "id": "<GUID>",
    "name": "<wallet name>",
    "version": 1,

    "keystore":
    {
      "ciphertext": "<the encrypted seed, lenght 32>",
      "mac": "<message authentication code, lenght 32>",

      "crypto":
      {
        "cipher": "aes-128-ctr",
        "cipherparams":
        {
          "iv": "<initialization vector, lenght 16>"
        }
      },

      "kdf":
      {
        "function": "argon2d",
        "parameters":
        {
          "p": "<degree of paralelism, defaults to 1>",
          "msize": "<The amount of memory (in Kilobytes) to use to calculate the hash, defaults to 8192>",
          "i": "<The number of iterations to perform to compute the hash. Because of Argon2's higher security, huge values like with PBKDF2 are not as necessary, although multiple iterations are still very much recommended, defaults to 40>",
          "salt": "<random salt passed to kdf, lenght 16>",
          "adata": "<null>",
          "ksecret": "<null>",
          "dklen": "<length for the derived key. Must be >= 32, defaults to 32>"
        }
      }
    }
  }
]
