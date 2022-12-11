using System.Runtime.Serialization;

namespace EHealthCard.Models
{
    [DataContract]
    public class DataPointLine
    {
        public DataPointLine(string label, double y)
        {
            this.label = label;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string? label = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}
