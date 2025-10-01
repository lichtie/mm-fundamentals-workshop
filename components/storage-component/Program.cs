using System.Threading.Tasks;
using Pulumi;

class Program
{
    public static Task Main(string []args) =>
        Pulumi.Experimental.Provider.ComponentProviderHost.Serve(args);
}