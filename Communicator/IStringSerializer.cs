namespace Communicator
{
    public interface IStringSerializer
	{
		string Serialize<T>(T cSharpObj);
	}

	public interface IStringDeserializer
	{
		T Deserialize<T>(string serializedObj);
	}

	public interface IDataSerializer : IStringSerializer, IStringDeserializer
	{
		
	}
}