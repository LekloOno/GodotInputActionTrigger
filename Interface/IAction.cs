namespace GIAT.Interface;

public interface IAction<IInput>
{
    bool Do(IInput input);
}