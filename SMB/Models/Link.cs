namespace SMB.Models
{
    public class Link
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public int? SubjectId { get; set; }
        public int? TopicId { get; set; }

        public Subject subject { get; set; }
        public Topic Topic { get; set; }
    }
}