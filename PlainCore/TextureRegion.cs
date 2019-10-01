namespace PlainCore
{
    public class TextureRegion
    {
        public TextureRegion(FloatRect region, Texture2D texture)
        {
            Region = region;
            Texture = texture;
        }

        public FloatRect Region { get; }
        public Texture2D Texture { get; }

        public override bool Equals(object obj)
        {
            return obj is TextureRegion region &&
                   Region.Equals(region.Region) &&
                   Texture.Equals(region.Texture);
        }
        public override int GetHashCode()
        {
            var hashCode = -51722311;
            hashCode = hashCode * -1521134295 + Region.GetHashCode();
            hashCode = hashCode * -1521134295 + Texture.GetHashCode();
            return hashCode;
        }
    }
}
