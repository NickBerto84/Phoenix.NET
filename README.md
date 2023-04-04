## Phoenix.NET ##
<br><br>
Phoenix.NET is a TCP-based, Pub/Sub C# library. It is developed to allow an easy-to-use, channel-separated communication on a LAN infrastructure.
<br><br>
The library provides a server and two client types: one for network communication and one for local communication.
<br>
The client types are also called *nodes* and they implement the [**IPhoenixNode**](/Phoenix.NET/Phoenix.NET.Common/Nodes/IPhoenixNode.cs) interface, which defines the pub/sub methods (basically: *Publish*, *Subscribe* and *Unsubscribe*).
<br><br>
The **PhoenixClientNode** type is responsible for the network communication and talks to the **PhoenixServer** through a socket connection. It can be found in the *Phoenix.NET.Client* namespace.
<br><br>
The **PhoenixLocalNode** type, instead, is used to communicate locally (without sockets) with the same pub/sub infrastructure of the server. It can be found in the *Phoenix.NET.Server* 

The **PhoenixServer** type handles the connections coming from the client nodes. This can be found in the *Phoenix.NET.Server* namespace.
