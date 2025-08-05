namespace DotNetIdentity.Models
{
    /// <summary>
    /// Gender enum
    /// </summary>
    public enum Gender { 
        ///<summary>gender unknown</summary>
        Unknown, 
        ///<summary>gender male</summary>
        Male, 
        ///<summary>gender female</summary>
        Female }
    /// <summary>
    /// Department enum
    /// </summary>
    public enum UserType { 
        ///<summary>Admin</summary>
        Admin, 
        ///<summary>User Connected</summary>
        User}
    /// <summary>
    /// TwoFactorType enum
    /// </summary>
    public enum TwoFactorType { 
        ///<summary>2fa type none</summary>
        None, 
        ///<summary>2fa type email</summary>
        Email, 
        ///<summary>2fa type Authenticator app</summary>
        Authenticator
    }
}