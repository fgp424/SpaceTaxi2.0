using DIKUArcade.EventBus;

    public static class TaxiBus { 
        private static GameEventBus<object> eventBus;
        public static GameEventBus<object> GetBus() {
            return TaxiBus.eventBus ?? (TaxiBus.eventBus = 
            new GameEventBus<object>()); 
        }
    }   

