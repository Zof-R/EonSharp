# EonSharp

EonSharp is a .Net integration library for [Eon's](https://github.com/EonTechnology) JsonRpc Api, at the time of writing at version 0.8.0 of the Eon blockchain.
The library provides an object oriented abstraction of the underlying Api and implements support for both the documented and undocumented Api calls as of 4th of February of 2018.
The supported Api features include Account generation, Transaction processing, node specific calls to obtain metrics, Blocks and Transactions, Multi Signature and Colored Coins.
The library also implements a simple/pure DI approach using a ClassMapper class for custom extensibility.

## Resources

You can find information and Api documentation related to Eon Technology at https://eontechnology.org/


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
    
    var logger = m_eonClient.TransportContext as EonSharp.Logging.ILog;
    logger.LogChanged += (s, e) => Console.WriteLine(e.ToString());
    
    //Needs to be called at least once to update internal variables related to blockchain state.
    await eonClient.UpdateBlockchainDetails();
```

### Creating an account

```csharp
    //Default constructor generates a new seed.
    //To load an existing account just pass the seed to the constructor and all account related info will be derived from that seed.
    var account = new EonSharp.Generators.AccountGenerator();
    string acId = account.AccountId;
    long acNmb = account.AccountNumber;
    string acPriv = account.PrivateKeyToString(); //Equals seed
    string acPub = account.PublicKeyToString();
    string acExpd = account.ExpandedPrivateKeyToString(); //combined priv+pub keys
```

### Processing a Transaction

```csharp
    //Creates a DepositRefill transaction object providing an account id and amount in microeons
    var refill = new EonSharp.Api.Transactions.DepositRefill(account.AccountId, 10);
    //Signs the Transaction
    refill.SignTransaction(account.ExpandedPrivateKey);
    //Sends the Transaction to the network  
    await eonClient.Bot.Transactions.PutTransactionAsync(refill);
```


## Documentation

It's still a work in progress but will be available soon.


## Changelog

You can check the full [changelog](CHANGELOG.md)



## Thanks and Credits

To @acidburn and @gassman for testing and suggestions, also some of the IdProvider class methods where initially based on @gassman's port from java to c# of the EonTechnology Format.java src.



## Acknowledgements

The EonSharp library uses code from the following external dependencies:

- [Chaos.NaCl by CodesInChaos.](https://github.com/CodesInChaos/Chaos.NaCl)
- [BEncode source originally posted at http://snipplr.com/view/37790/ by SuprDewd.](http://snipplr.com/view/37790/)
- [Json.NET by Newtonsoft](https://www.newtonsoft.com/json)



## License

The EonSharp library is licensed under the terms of the [GPL Open Source license](LICENSE) and is available for free.

