namespace UI_Interface.Helpers
{
    /// <summary>
    /// Перечисление, представляющее типы результатов входа.
    /// </summary>
    public enum LoginResultType
    {
        Success,
        Unauthorized,
        CancelledByUser,
        UnknownError
    }
}
