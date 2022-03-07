namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcesarEvent(string mensaxe);
    }
}