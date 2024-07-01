using System.Collections.Generic;

namespace OmsSolution.Models
{

    public class SubItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }

    public class MenuItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public List<SubItem> SubItems { get; set; }
    }


}
