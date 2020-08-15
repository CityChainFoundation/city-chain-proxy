using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using City.Chain.Proxy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace City.Chain.Proxy.Controllers
{
    [Route(".well-known/blockcore/node")]
    public class WellKnownController : ControllerBase
    {
        private readonly NodeService node;
        private HttpProxyOptions options;

        public WellKnownController(NodeService node)
        {
            this.node = node;

            options = HttpProxyOptionsBuilder.Instance.WithBeforeSend((c, hrm) =>
            {
                hrm.Headers.Add("Node-Api-Key", node.NodeApiKey);
                return Task.CompletedTask;
            }).Build();
        }

        /// <summary>
        /// Returns the identity of the node.
        /// </summary>
        /// <returns></returns>
        [HttpGet("storage/schemas")]
        public Task GetSchemaVersions()
        {
            return this.HttpProxyAsync($"{node.NodeApiUrl}/.well-known/blockcore/node/storage/schemas", options);
        }

        /// <summary>
        /// Returns the identity of the node.
        /// </summary>
        /// <returns></returns>
        [HttpGet("identity")]
        public Task GetNodeIdentity()
        {
            return this.HttpProxyAsync($"{node.NodeApiUrl}/.well-known/blockcore/node/identity", options);
        }
    }
}
