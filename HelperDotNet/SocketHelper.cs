using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperDotNet
{
    public sealed class SocketHelper
    {
        public sealed class ClientSocket
        {
            public Protocol protocol;

            public ClientSocket()
            {

            }
        }

        public sealed class ServerSocket
        {
            public Protocol protocol;

            public ServerSocket()
            {

            }
        }

        public sealed class Protocol
        {
            public List<Command> commands;

            public Protocol()
            {

            }
        }

        public sealed class Command
        {
            public List<Command> subCommands;

            public Command()
            {

            }
        }
    }
}
