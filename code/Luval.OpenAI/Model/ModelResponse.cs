using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Models
{
    public class ModelResponse
    {
        public ModelResponse()
        {
            Data = new List<Model>();
        }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("data")]
        public IList<Model> Data { get; set; }
    }

    public class ModelPermission
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("allow_create_engine")]
        public bool AllowCreateEngine { get; set; }

        [JsonProperty("allow_sampling")]
        public bool AllowSampling { get; set; }

        [JsonProperty("allow_logprobs")]
        public bool AllowLogprobs { get; set; }

        [JsonProperty("allow_search_indices")]
        public bool AllowSearchIndices { get; set; }

        [JsonProperty("allow_view")]
        public bool AllowView { get; set; }

        [JsonProperty("allow_fine_tuning")]
        public bool AllowFineTuning { get; set; }

        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("group")]
        public object Group { get; set; }

        [JsonProperty("is_blocking")]
        public bool IsBlocking { get; set; }
    }

    public class Model
    {

        #region Constructors

        public Model()
        {

        }

        public Model(string id)
        {
            Id = id;
            OwnedBy = "openai";
        }

        #endregion

        public override string ToString()
        {
            return Id;
        }


        public static Model TextDavinci003 => new Model("text-davinci-003")
        {
            OwnedBy = "openai-internal",
            Object = "model",
            CreatedUnixTime = 1669599635,
            Root = "text-davinci-003"
        };

        public static Model GPTTurbo => new Model("gpt-3.5-turbo")
        {
            OwnedBy = "openai",
            Object = "model",
            CreatedUnixTime = 1677610602,
            Root = "gpt-3.5-turbo"
        };

        public static Model GPTTurbo16k => new Model("gpt-3.5-turbo-16k-0613")
        {
            OwnedBy = "openai",
            Object = "model",
            CreatedUnixTime = 1685474247,
            Root = "gpt-3.5-turbo-16k-0613"
        };

        public static Model Gpt4 => new Model("gpt-4")
        {
            OwnedBy = "openai",
            Object = "model",
            CreatedUnixTime = 1677610602,
            Root = "gpt-4"
        };

        public static Model Gpt432k => new Model("gpt-4-32k")
        {
            OwnedBy = "openai",
            Object = "model",
            CreatedUnixTime = 1677610602,
            Root = "gpt-4"
        };

        public static Model GPTTurbo0301 => new Model("gpt-3.5-turbo-0301")
        {
            OwnedBy = "openai",
            Object = "model",
            CreatedUnixTime = 1677649963,
            Root = "gpt-3.5-turbo-0301"
        };

        public static Model Whisper1 => new Model("whisper-1")
        {
            OwnedBy = "openai-internal",
            Object = "model",
            CreatedUnixTime = 1677532384,
            Root = "whisper-1"
        };


        #region Property Implementation

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        public long? CreatedUnixTime { get; set; }

        [JsonIgnore]
        public DateTime? Created => CreatedUnixTime.ToDateTimeFromUnix();

        [JsonProperty("owned_by")]
        public string OwnedBy { get; set; }

        [JsonProperty("permission")]
        public ModelPermission[] Permission { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }

        [JsonProperty("parent")]
        public object Parent { get; set; }


        #endregion


    }

    public static class ModelMaxTokens
    {
        public static Dictionary<string, int> Instance => new Dictionary<string, int>()
                                                            {
                                                                {"gpt-4", 8192},
                                                                {"gpt-4-0314", 8192},
                                                                {"gpt-4-32k", 32768},
                                                                {"gpt-4-32k-0314", 32768},
                                                                {"gpt-3.5-turbo", 4096},
                                                                {"gpt-3.5-turbo-0301", 4096},
                                                                {"gpt-3.5-turbo-16k-0613", 16384},
                                                                {"text-davinci-003", 4097},
                                                                {"text-davinci-002", 4097},
                                                                {"code-davinci-002", 8001},
                                                                {"davinci", 2048},
                                                                {"curie", 2048},
                                                                {"babbage", 4096},
                                                                {"ada", 4096},
                                                            };
    }

}
