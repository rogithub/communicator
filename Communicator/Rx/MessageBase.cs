
namespace Communicator.Rx
{
    public abstract class MessageBase<D, M> : IMessage<D, M> where M: new()
    {
        
        public D Data { get; set; }
        public M MetaData { get; set; }        


        public MessageBase(D data, M metaData)
        {
            this.Data = data;
            this.MetaData = metaData;
        }
    }

}
