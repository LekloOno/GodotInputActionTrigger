namespace GIAT.Interface;

public interface IAction<T>
{
    bool Do(T input);
}

public interface IAction
{
    bool Do();
}