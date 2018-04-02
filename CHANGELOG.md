# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Stream support in wallet serialization
- Boolean implicit operator to State class based on HTTP rules
- Information, CommitedTransactions and UncommitedTRansactions properties to Wallet class
- Polling methods to Wallet class
- PuTransactionAsync method to Wallet class
- virtual methods to base class Transaction
- Balance to Wallet class
- Note to Transaction class
- Peer.Snapshot interface and methods
- EncryptMessage and DecryptMessage to Keystore Interface and classes

### Changed
- Refactored Wallet class
- Moved json serialization to ExtensionMethods class
- SalientAttributes, renamed to Attributes
- EONBalance, renamed to Balance
- Refactoring of transaction classes to keep in par with version 2.0 of the Eon API
- IdProvider core function changed to keep in par with version 2.0 of the Eon API
- Bot.ColoredCoin.GetInfo method signature changed to keep in par with version 2.0 of the Eon API
- Serialization of Wallet class to allow for partial AccountDetails persistence

### Fixed
- PublicKeyPairGenerator
- Blocks, missing property
- Info, missing properties and fix deserialization of a few others
- ColoredInfo, renaming to ColoredCoinInfo and fix deserialization


## [0.13.0] - 2018-02-18
### Added
- Filtering to HttpTransportLogger
- Wallet class
- Keystore classes and interfaces related to initial implementation of wallet encryption proposal
- PublicAccountGenerator and PublicKeyPairGenerator classes

### Fixed
- BEncoding in AccountRegistration



## [0.12.0] - 2018-02-04
- Initial commit