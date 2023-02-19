public class DialogButtonPressed: Message  {
    public enum Type{
        Submit, Left, Right, Up, Down, LeftClick, RightClick
    }

    public Type button;
    
}