namespace ActinUranium.Web.Helpers
{
    public sealed class WeightedLottery<T> : Lottery<T>
    {
        public void Add(T element, int weighting)
        {
            for (int i = 0; i < weighting; i++)
            {
                Add(element);
            }
        }
    }
}
