namespace GIAT.Interface;

public interface IAction<T> where T: IInput
{
    bool Do(T input);
}

public interface IAction
{
    bool Do();
}