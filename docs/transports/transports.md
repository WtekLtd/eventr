# Transports

Transport libraries contain logic specific to a particular event transport. They may contain event sources, event producers or both. At least one transport library is needed in order to specify how events are introduced into the system when the debugger is not installed.

## Generic

- [Development Transport](./development-transport.md)

## Amazon Web Services

- [SQS Transport](./sqs-transport.md)
- [EventBridge Transport](./eventbridge-transport.md)