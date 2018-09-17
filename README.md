# AutoDI

Very simple auto-registration for dotnet core DI.

Example usage:

```
  services
    .AddAutoDI( typeof( Startup ).Assembly, x => x.WithOpenGenericWireups().Transient() );
```
