namespace GymApp.Configurations.Entities
{
    public class GuidsSeederDefinition
    {
        public Guid SuperAdminGuid { get; set; } = new Guid("167af088-83bf-4770-b2e7-90bcab44a837");
        public Guid SupeAdminRuleGuid { get; set; } = new Guid("79d54256-912f-43fd-a014-38c007b8777e");
        public Guid GymAdminGuid { get; set; } = new Guid("517ef1d8-6ae9-417e-a50b-43393c317efa");
        public Guid GymAdminRuleGuid { get; set; } = new Guid("ab88a4ed-ba01-4f69-b1a3-9c1a8ebeea6b");
        public Guid TrainerGuid { get; set; } = new Guid("28c252f0-8a9d-456f-8525-cd0d3da6c854");
        public Guid TrainerRuleGuid { get; set; } = new Guid("b4537842-50de-4734-a878-00e1b3a5639c");
        public Guid TraineeGuid { get; set; } = new Guid("6d741454-2f17-416f-b7be-58df8f787fc1");
        public Guid TraineeRuleGuid { get; set; } = new Guid("0d816df9-a2b3-4a6b-a111-69ce4a5d1332");

        // Gym Guid
        public Guid GymGuid { get; set; } = new Guid("7ad3cd46-880d-4412-8a1d-6c71399fe4c0");
    }
}
