using System;

namespace Library.Misc
{
    public class RandomData
    {
        private readonly Random random = new Random();

        public string GetRandomString()
        {
            return Guid.NewGuid().ToString();
        }

        public DateTime GetRandomDate()
        {
            return DateTime.Today.AddDays(random.Next(0, 180));
        }

        public int GetRandomInt()
        {
            return random.Next(1, 100);
        }

        public bool GetRandomBoolean()
        {
            return Convert.ToBoolean(random.Next(0, 2));
        }
    }
}
