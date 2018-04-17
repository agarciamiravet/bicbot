namespace BIC.Bot.RestService.Enums
{
    public enum HomeActions
    {
        //Light
        LightOnKitchen = 1,
        LightOffKitchen = 2,
        LightOnBedroom = 3,
        LightOffBedroom = 4,
        LightOnBathRoom = 5,
        LightOffBathRoom = 6,
        LightOnEntranceHall = 7,
        LightOffEntranceHall = 8,
        LightOnAll = 9,
        LightOffAll = 10,

        //Blind
        BlindUpBedroom = 1,
        BlindDownBedroom = 2,
        BlindUpAll = 3,
        BlindUpKitchen = 4,
        BlindDownKitchen = 5,
        BlindDownAll = 6,
        BlindUpMailBox = 7,
        BlindDownMailbox = 8,
        MailUser = 9,



        //Temperatura
        GetTemperature = 11,

        //messenger
        
    }

    public enum TypeActions
    {
        Light = 0,
        Blinf = 1
    }

}