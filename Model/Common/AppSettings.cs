using System.Text.Json.Nodes;

namespace Wee_XYYY.Model.Common;

public class AppSettings
{
    public static void Init(IConfiguration configuration)
    {
        Configuration=configuration;
    }
    //所获取的节点的名字
    public static IConfiguration Configuration { get; set; }
}
