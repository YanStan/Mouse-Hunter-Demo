

using Newtonsoft.Json.Linq;
using Mouse_Hunter.Resources.AntiCaptcha.ApiResponse;

namespace Mouse_Hunter.Resources.AntiCaptcha
{
    public interface IAnticaptchaTaskProtocol
    {
        JObject GetPostData();
        TaskResultResponse.SolutionData GetTaskSolution();
    }
}
