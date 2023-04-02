using Luval.OpenAI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public class BaseModelRequest
    {
        protected BaseModelRequest(Model model)
        {
            Model = model;        }

        [JsonIgnore]
        public virtual Model Model { get; set; }

        [JsonProperty("model")]
        public virtual string ModelID { get { return Model.Id; } }
    }
}
