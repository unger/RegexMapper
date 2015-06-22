namespace RegexMapper.Tests.Model
{
    using System;

    public class TestModel : IEquatable<TestModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Equals(TestModel other)
        {
            return this.Id.Equals(other.Id) && string.Equals(this.Name, other.Name);
        }
    }
}