# **EonSharp**

EonSharp is a .Net integration library for **[Eon's](https://github.com/EonTechnology)** JsonRpc Api and it provides an object oriented abstraction of the underlying Api.
The supported Api features include Account generation, Transaction processing, node specific calls to obtain metrics, Blocks and Transactions, Multi Signature and Colored Coins.
The library also implements a simple/pure DI approach using a ClassMapper class for custom extensibility.

## Resources

You can find information and Api documentation related to **Eon Technology** at https://eontechnology.org/

For detailed **EonSharp** Api documentation click [here](api/EonSharp.yml)


## Simple usage example

### initialization

```csharp
    //Usefull only during beta. Default is false. Afects all transport contexts for now.
    EonSharp.Configuration.IgnoreSslErrors = true;
    
    //Example of logging class injection to default EonClient's object graph construction
    EonClient.ClassMapper[typeof(EonSharp.Network.ITransportContext)] = new ActivatorDescriptor[]
    {
        new ActivatorDescriptor(typeof(EonSharp.Network.Transports.HttpTransportClient)),
        new ActivatorDescriptor(typeof(EonSharp.Logging.HttpTransportLogger), new object[]{ "[HTTP TRANSPORT] " })
    };
    
    //Instantiation of EonClient root class. All calls are made through this class.
    //Default constructor uses testnet address, this default will change at mainnet launch.
    var eonClient = new EonClient();
    
    //As HttpTransportLogger was injected in the object graph for type ITransportContext
    //and it implements ILog we can cast the TransportContext instance to ILog
    var logger = m_eonClient.TransportContext as EonSharp.Logging.ILog;
    logger.LogChanged += (s, e) => Console.WriteLine(e.ToString());
    
    //Needs to be called at least once to update internal variables related to blockchain state.
    await eonClient.UpdateBlockchainDetails();
```

### Creating an account

```csharp
    //Default constructor generates a new seed.
    //To load an existing account just pass the seed to the constructor
    //and all account related info will be derived from that seed.
    var account = new EonSharp.Generators.AccountGenerator();

    Console.WriteLine($"AccountId:      {account.AccountId}");
    Console.WriteLine($"Account Number: {account.AccountNumber}");
    Console.WriteLine($"Private Key:    {account.PrivateKeyToString()}"); //Equals seed
    Console.WriteLine($"Public Key:     {account.PublicKeyToString()}"); 
    Console.WriteLine($"Expanded Key:   {account.ExpandedPrivateKeyToString()}"); //combined priv+pub keys
```

### Processing a Transaction

```csharp
    //Creates a DepositRefill transaction object providing an account id and amount in microeons
    var refill = new EonSharp.Api.Transactions.Deposit(account.AccountId, 10);

    //Signs the Transaction
    refill.SignTransaction(account.ExpandedPrivateKey);

    //Sends the Transaction to the network  
    await eonClient.Bot.Transactions.PutTransactionAsync(refill);
```

