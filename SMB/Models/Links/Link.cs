namespace SMB.Models.Links
{
    public class Link
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }
    }
}