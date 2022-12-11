using System.Runtime.Serialization;
using System.Xml.Linq;

namespace EHealthCard.Models
{
    //DataContract for Serializing Data - required to serve in JSON format
    [DataContract]
    public class HospitalizationTableRecord
    {
        public HospitalizationTableRecord(string name, int first, int second, int third, 
            int fourth, int fifth, int sixth, int seventh, int eighth, 
            int ninth, int tenth, int eleventh, int twelveth ) 
        {
            this.name = name;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
            this.tenth = tenth;
            this.eleventh = eleventh;
            this.twelveth = twelveth;
        }
        public string name { get; set; }
        public int first { get; set; }
        public int second { get; set; }
        public int third { get; set; }
        public int fourth { get; set; }
        public int fifth { get; set; }
        public int sixth { get; set; }
        public int seventh { get; set; }
        public int eighth { get; set; }
        public int ninth { get; set; }
        public int tenth { get; set; }
        public int eleventh { get; set; }
        public int twelveth { get; set; }
    }
}
