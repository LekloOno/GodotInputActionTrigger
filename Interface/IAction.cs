namespace GIAT.Interface;

public interface IAction<T> where T: class, IInput
{
    bool Do(T input);
}

public interface IAction
{
    bool Do();
}