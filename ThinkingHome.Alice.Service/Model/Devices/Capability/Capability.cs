namespace ThinkingHome.Alice.Service.Model.Devices.Capability
{
    /// <summary>
    /// Общий интерфейс умения
    /// </summary>
    /// <typeparam name="TParams">Тип поля params</typeparam>
    /// <typeparam name="TState">Тип поля state</typeparam>
    public abstract class Capability<TParams, TState> where TState : CapabilityStateModel
    {
        protected abstract CapabilityType Type { get; }
        protected abstract bool Retrievable { get; }
        protected abstract TParams Params { get; }
        protected abstract TState GetState();

        public CapabilityInfoModel GetDescription()
        {
            return new CapabilityInfoModel
            {
                type = Type,
                retrievable = Retrievable,
                parameters = Params
            };
        }

        public CapabilityState GetStateResponse()
        {
            return new CapabilityState
            {
                type = Type,
                state = GetState()
            };
        }
    }
}