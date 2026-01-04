namespace CoreServiceContracts.Hub.Menu
{
    public class MenuDO
    {
        public string Caption { get; set; }
        public List<MenuDO> Items { get; set; } = new();
    }
}
