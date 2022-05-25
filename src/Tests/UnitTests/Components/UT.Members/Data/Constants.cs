// ReSharper disable All
namespace UT.Members.Data
{
  internal static class Constants
  {
    public const string TEST_ASSEMBLY = "../../../../../bin.tests/TestLibrary.dll";

    #region Enum

    public const string PUBLIC_ENUM = "EnumTypePublic";
    public const string INTERNAL_ENUM = "EnumTypeInternal";
    public const string PUBLIC_NESTED_ENUM = "NestedEnumPublic";
    public const string PROTECTED_NESTED_ENUM = "NestedEnumProtected";
    public const string PROTECTED_INTERNAL_NESTED_ENUM = "NestedEnumProtectedInternal";
    public const string INTERNAL_NESTED_ENUM = "NestedEnumInternal";

    #endregion

    #region Interface

    public const string PUBLIC_INTERFACE = "IInterfaceTypePublic";
    public const string INTERNAL_INTERFACE = "IInterfaceTypeInternal";
    public const string PUBLIC_NESTED_INTERFACE = "INestedInterfacePublic";
    public const string PROTECTED_NESTED_INTERFACE = "INestedInterfaceProtected";
    public const string PROTECTED_INTERNAL_NESTED_INTERFACE = "INestedInterfaceProtectedInternal";
    public const string INTERNAL_NESTED_INTERFACE = "INestedInterfaceInternal";
    public const string PUBLIC_INHERITED_INTERFACE = "IInheritedInterface";
    public const string PUBLIC_INHERITING_AND_INHERITED_INTERFACE = "IInheritingAndInheritedInterface";
    public const string PUBLIC_INHERITING_INTERFACE = "IInheritingInterface";
    public const string PUBLIC_INHERITING_INTERFACE_EMPTY = "IInheritingInterfaceEmpty";
    public const string PUBLIC_INHERITING_COMPLEX_INTERFACE = "IInheritingInterfaceComplex";
    public const string PUBLIC_INHERITING_COMPLEX_INTERFACE_EMPTY = "IInheritingInterfaceComplexEmpty";
    public const string PUBLIC_GENERIC_INTERFACE = "IGenericInterface";
    public const string PUBLIC_NESTED_GENERIC_INTERFACE = "INestedGenericInterface";

    #endregion

    #region Struct

    public const string STRUCT_NAMESPACE = "TestLibrary.Structs";
    public const string PUBLIC_STRUCT = "StructTypePublic";
    public const string PUBLIC_STRUCT_READONLY = "StructReadonlyType";
    public const string INTERNAL_STRUCT = "StructTypeInternal";
    public const string PUBLIC_NESTED_STRUCT = "NestedStructPublic";
    public const string PROTECTED_NESTED_STRUCT = "NestedStructProtected";
    public const string PROTECTED_INTERNAL_NESTED_STRUCT = "NestedStructProtectedInternal";
    public const string INTERNAL_NESTED_STRUCT = "NestedStructInternal";
    public const string PUBLIC_GENERIC_STRUCT = "GenericStruct";
    public const string PUBLIC_NESTED_GENERIC_STRUCT = "NestedGenericStruct";
    public const string PUBLIC_INHERITING_STRUCT = "InheritingStruct";
    public const string PUBLIC_INHERITING_STRUCT_EMPTY = "InheritingStructEmpty";
    public const string PUBLIC_INHERITING_COMPLEX_STRUCT = "InheritingStructComplex";
    public const string PUBLIC_INHERITING_COMPLEX_STRUCT_EMPTY = "InheritingStructComplexEmpty";
    public const string PUBLIC_STRUCT_PARAM_CTOR = "StructWithParamConstructor";
    public const string PUBLIC_STRUCT_EMPTY_CTOR = "StructEmptyConstructor";

    #endregion
    
    #region Record
    
    public const string RECORD_NAMESPACE = "TestLibrary.Records";
    public const string PUBLIC_RECORD_SEALED = "RecordTypePublicSealed";
    public const string PUBLIC_RECORD_ABSTRACT = "RecordTypePublicAbstract";
    public const string PUBLIC_RECORD = "RecordTypePublic";
    public const string PUBLIC_RECORD_READONLY = "RecordReadonlyType";
    public const string INTERNAL_RECORD = "RecordTypeInternal";
    public const string PUBLIC_NESTED_RECORD = "NestedRecordPublic";
    public const string PROTECTED_NESTED_RECORD = "NestedRecordProtected";
    public const string PROTECTED_INTERNAL_NESTED_RECORD = "NestedRecordProtectedInternal";
    public const string INTERNAL_NESTED_RECORD = "NestedRecordInternal";
    public const string PUBLIC_GENERIC_RECORD = "GenericRecord";
    public const string PUBLIC_NESTED_GENERIC_RECORD = "NestedGenericRecord";
    public const string PUBLIC_INHERITING_RECORD = "InheritingRecord";
    public const string PUBLIC_INHERITING_RECORD_EMPTY = "InheritingRecordEmpty";
    public const string PUBLIC_INHERITING_COMPLEX_RECORD = "InheritingRecordComplex";
    public const string PUBLIC_INHERITING_COMPLEX_RECORD_EMPTY = "InheritingRecordComplexEmpty";
    public const string PUBLIC_RECORD_PARAM_CTOR = "RecordWithParamConstructor";
    public const string PUBLIC_RECORD_EMPTY_CTOR = "RecordEmptyConstructor";
    
    #endregion

    #region Class

    public const string PUBLIC_CLASS = "ClassTypePublic";
    public const string PUBLIC_CLASS_ABSTRACT = "ClassTypePublicAbstract";
    public const string PUBLIC_CLASS_SEALED = "ClassTypePublicSealed";
    public const string PUBLIC_CLASS_STATIC = "ClassTypePublicStatic";
    public const string INTERNAL_CLASS = "ClassTypeInternal";
    public const string PUBLIC_NESTED_CLASS = "NestedClassPublic";
    public const string PROTECTED_NESTED_CLASS = "NestedClassProtected";
    public const string PROTECTED_INTERNAL_NESTED_CLASS = "NestedClassProtectedInternal";
    public const string INTERNAL_NESTED_CLASS = "NestedClassInternal";
    public const string PUBLIC_GENERIC_CLASS = "GenericClass";
    public const string PUBLIC_NESTED_GENERIC_CLASS = "NestedGenericClass";
    public const string PUBLIC_INHERITING_CLASS = "InheritingClass";
    public const string PUBLIC_INHERITING_CLASS_EMPTY = "InheritingClassEmpty";
    public const string PUBLIC_INHERITING_CLASS_BASE = "InheritingClassWithBase";
    public const string PUBLIC_INHERITING_CLASS_BASE_EMPTY = "InheritingClassWithBaseEmpty";
    public const string PUBLIC_INHERITING_COMPLEX_CLASS = "InheritingClassComplex";
    public const string PUBLIC_INHERITING_COMPLEX_CLASS_EMPTY = "InheritingClassComplexEmpty";
    public const string PUBLIC_INHERITING_COMPLEX_CLASS_BASE = "InheritingClassComplexWithBase";
    public const string PUBLIC_INHERITING_COMPLEX_CLASS_BASE_EMPTY = "InheritingClassComplexWithBaseEmpty";
    public const string PUBLIC_CLASS_COMPLEX_INHERITANCE = "ClassComplexInheritance";
    public const string PUBLIC_CLASS_DEFAULT_CTOR = "ClassWithDefaultConstructor";
    public const string PUBLIC_CLASS_PARAM_CTOR = "ClassWithParamConstructor";

    #endregion

    #region Properties

    public const string PROPERTIES_NAMESPACE = "TestLibrary.Members.Properties";
    public const string PROPERTIES_CLASS = "ClassProperties";
    public const string PROPERTIES_RECORD = "RecordProperties";
    public const string PROPERTIES_CLASS_ABSTRACT = "ClassPropertiesAbstract";
    public const string PROPERTY_NORMAL = "PropertyNormal";
    public const string PROPERTY_VIRTUAL = "PropertyVirtual";
    public const string PROPERTY_ABSTRACT = "PropertyAbstract";
    public const string PROPERTY_OVERRIDE = "PropertyOverride";
    public const string PROPERTIES_STRUCT = "StructProperties";
    public const string PROPERTIES_INTERFACE = "IInterfaceProperties";
    public const string PROPERTY_STATIC = "PropertyStatic";
    public const string PROPERTY_GET_INIT = "PropertyGetInit";
    public const string PROPERTY_GET_SET = "PropertyGetSet";
    public const string PROPERTY_GET = "PropertyGet";
    public const string PROPERTY_SET = "PropertySet";
    public const string PROPERTY_PUBLIC = "PropertyPublic";
    public const string PROPERTY_PROTECTED = "PropertyProtected";
    public const string PROPERTY_INTERNAL = "PropertyInternal";
    public const string PROPERTY_PRIVATE = "PropertyPrivate";
    public const string PROPERTY_PROTECTED_INTERNAL = "PropertyProtectedInternal";
    public const string PROPERTY_PROTECTED_INTERNAL_GET_PRIVATE = "PropertyProtectedInternalGetPrivate";
    public const string PROPERTY_PROTECTED_INTERNAL_SET_PRIVATE = "PropertyProtectedInternalSetPrivate";
    public const string PROPERTY_PUBLIC_GET_PROTECTED = "PropertyPublicGetProtected";
    public const string PROPERTY_PUBLIC_GET_INTERNAL = "PropertyPublicGetInternal";
    public const string PROPERTY_PUBLIC_GET_PRIVATE = "PropertyPublicGetPrivate";
    public const string PROPERTY_PUBLIC_SET_PROTECTED = "PropertyPublicSetProtected";
    public const string PROPERTY_PUBLIC_SET_INTERNAL = "PropertyPublicSetInternal";
    public const string PROPERTY_PUBLIC_SET_PRIVATE = "PropertyPublicSetPrivate";
    public const string PUBLIC_CLASS_PROPERTY_PARENT = "ClassPropertiesParent";
    public const string PROPERTY_PUBLIC_TOP = "PropertyTop";
    public const string PUBLIC_CLASS_PROPERTY_NESTED  = "ClassPropertiesNested";
    public const string PROPERTY_PUBLIC_NESTED = "PropertyNested";
    public const string PUBLIC_CLASS_PROPERTY_NESTED2  = "ClassPropertiesNestedNested";
    public const string PROPERTY_PUBLIC_NESTED2 = "PropertyNestedNested";
    public const string PROPERTY_READONLY = "PropertyReadonly";
    public const string PROPERTY_FULL_READONLY = "PropertyFullReadonly";
    public const string PROPERTY_FULL_READONLY_GET = "PropertyFullReadonlyGet";

    #endregion

    #region Events

    public const string EVENTS_NAMESPACE = "TestLibrary.Members.Events";
    public const string PUBLIC_CLASS_EVENT_PARENT = "ClassEventsParent";
    public const string PUBLIC_CLASS_EVENT_NESTED = "ClassEventsNested";
    public const string PUBLIC_CLASS_EVENT_NESTED2 = "ClassEventsNestedNested";
    public const string EVENT_PUBLIC_TOP = "EventTop";
    public const string EVENT_PUBLIC_NESTED = "EventNested";
    public const string EVENT_PUBLIC_NESTED2 = "EventNestedNested";
    public const string EVENTS_CLASS = "ClassEvents";
    public const string EVENTS_STRUCT = "StructEvents";
    public const string EVENTS_INTERFACE = "IInterfaceEvents";
    public const string EVENTS_CLASS_ABSTRACT = "ClassEventsAbstract";
    public const string EVENT_STATIC = "EventStatic";
    public const string EVENT_PUBLIC = "EventPublic";
    public const string EVENT_INTERNAL = "EventInternal";
    public const string EVENT_PROTECTED = "EventProtected";
    public const string EVENT_PROTECTED_INTERNAL = "EventProtectedInternal";
    public const string EVENT_NORMAL = "EventNormal";
    public const string EVENT_VIRTUAL = "EventVirtual";
    public const string EVENT_ABSTRACT = "EventAbstract";
    public const string EVENT_OVERRIDE = "EventOverride";

    #endregion

    #region Methods

    public const string METHODS_NAMESPACE = "TestLibrary.Members.Methods";
    public const string METHODS_CLASS_GENERIC = "ClassMethodsGenerics";
    public const string METHODS_CLASS = "ClassMethods";
    public const string METHODS_CLASS_ABSTRACT = "ClassMethodsAbstract";
    public const string METHODS_STRUCT = "StructMethods";
    public const string METHODS_INTERFACE = "IInterfaceMethods";
    public const string METHOD_STATIC = "MethodStatic";
    public const string METHOD_ASYNC = "MethodAsync";
    public const string METHOD_STRING = "MethodString";
    public const string METHOD_PUBLIC = "MethodPublic";
    public const string METHOD_INTERNAL = "MethodInternal";
    public const string METHOD_PROTECTED = "MethodProtected";
    public const string METHOD_PROTECTED_INTERNAL = "MethodProtectedInternal";
    public const string METHOD_NORMAL = "MethodNormal";
    public const string METHOD_OVERRIDE = "MethodOverride";
    public const string METHOD_VIRTUAL = "MethodVirtual";
    public const string METHOD_ABSTRACT = "MethodAbstract";
    public const string METHOD_ADDITION = "+";
    public const string METHOD_SUBSTRACTION = "-";
    public const string METHOD_MULTIPLY = "*";
    public const string METHOD_DIVISION = "/";
    public const string METHOD_MODULUS = "%";
    public const string METHOD_EXCLUSIVEOR = "^";
    public const string METHOD_BITWISEAND = "&";
    public const string METHOD_BITWISEOR = "|";
    public const string METHOD_LOGICALNOT = "!";
    public const string METHOD_LEFTSHIFT = "<<";
    public const string METHOD_RIGHTSHIFT = ">>";
    public const string METHOD_EQUALITY = "==";
    public const string METHOD_INEQUALITY = "!=";
    public const string METHOD_GREATERTHAN = ">";
    public const string METHOD_LESSTHAN = "<";
    public const string METHOD_GREATERTHANEQUALS = ">=";
    public const string METHOD_LESSTHANEQUALS = "<=";
    public const string METHOD_DECREMENT = "--";
    public const string METHOD_INCREMENT = "++";
    public const string METHOD_ONESCOMPLEMENT = "~";
    public const string METHOD_IMPLICIT = "int";
    public const string METHOD_EXPLICIT = "uint";
    public const string METHOD_GENERIC = "MethodGeneric";
    public const string METHOD_GENERIC_CONSTRAINTS = "MethodGenericConstraint";

    #endregion

    #region Delegates

    public const string DELEGATES_NAMESPACE = "TestLibrary.Members.Delegates";
    public const string DELEGATES_CLASS = "ClassDelegates";
    public const string DELEGATES_CLASS_GENERIC = "ClassDelegatesGenerics";
    public const string DELEGATES_PARENT = "ClassDelegatesParent";
    public const string DELEGATES_NESTED = "ClassDelegatesNested";
    public const string DELEGATES_NESTED2 = "ClassDelegatesNestedNested";
    public const string DELEGATE_TOP = "DelegateTop";
    public const string DELEGATE_NESTED = "DelegateNested";
    public const string DELEGATE_NESTED2 = "DelegateNestedNested";
    public const string DELEGATES_STRUCT = "StructDelegates";
    public const string DELEGATES_INTERFACE = "IInterfaceDelegates";
    public const string DELEGATE_PUBLIC = "DelegatePublic";
    public const string DELEGATE_ARGUMENTS = "DelegateArguments";
    public const string DELEGATE_STRING = "DelegateString";
    public const string DELEGATE_PROTECTED = "DelegateProtected";
    public const string DELEGATE_INTERNAL = "DelegateInternal";
    public const string DELEGATE_PROTECTED_INTERNAL = "DelegateProtectedInternal";
    public const string DELEGATE_GENERIC = "DelegateGeneric";
    public const string DELEGATE_GENERIC_CONSTRAINT = "DelegateGenericConstraint";

    #endregion

    #region Arguments

    public const string ARGUMENTS_NAMESPACE = "TestLibrary.Members.Arguments";
    public const string ARGUMENTS_CLASS = "ClassArguments";
    public const string METHOD_PREFIX_FORMAT = "Method{0}";
    public const string DELEGATE_PREFIX_FORMAT = "Delegate{0}";

    public static string GetDelegate(string name)
      => string.Format(DELEGATE_PREFIX_FORMAT, name);
    public static string GetMethod(string name)
      => string.Format(METHOD_PREFIX_FORMAT, name);

    public const string ARGUMENT_ONE = "OneArg";
    public const string ARGUMENT_TWO = "TwoArg";
    public const string ARGUMENT_MODIFIERS = "ModArgs";
    public const string ARGUMENT_PARAMATERS = "ParamArg";

    #endregion

    public const string RES_TYPES_NAMESPACE = "TestLibrary.ResTypes";

    #region ResTypes

    public const string RES_TYPE_CLASS = "ClassResTypesValueTypes";
    public const string METHOD_RES_BYTE = "MethodByte";
    public const string METHOD_RES_SBYTE = "MethodSbyte";
    public const string METHOD_RES_CHAR = "MethodChar";
    public const string METHOD_RES_BOOL = "MethodBool";
    public const string METHOD_RES_STRING = "MethodString";
    public const string METHOD_RES_OBJECT = "MethodObject";
    public const string METHOD_RES_SHORT = "MethodShort";
    public const string METHOD_RES_USHORT = "MethodUShort";
    public const string METHOD_RES_INT = "MethodInt";
    public const string METHOD_RES_UINT = "MethodUInt";
    public const string METHOD_RES_LONG = "MethodLong";
    public const string METHOD_RES_ULONG = "MethodULong";
    public const string METHOD_RES_FLOAT = "MethodFloat";
    public const string METHOD_RES_DOUBLE = "MethodDouble";
    public const string METHOD_RES_DECIMAL = "MethodDecimal";
    public const string METHOD_RES_DYNAMIC = "MethodDynamic";
    public const string METHOD_RES_REF_STRING = "MethodRefString";
    public const string METHOD_RES_OBJR_DYNA = "MethodObjRetDynArg";
    public const string METHOD_RES_DYNR_OBJA = "MethodDynRetObjArg";
    public const string METHOD_RES_OBJR_MIXA = "MethodObjRetMixArgs";
    public const string METHOD_RES_DYNR_MIXA = "MethodDynRetMixArgs";

    public const string RES_TYPES_CLASS = "ClassResTypes";
    public const string METHOD_RES_NESTED_GENERIC_RET = "MethodNestedGenericRet";
    public const string METHOD_RES_NESTED_RET = "MethodNestedRet";
    public const string METHOD_RES_NESTED_GENERIC_VALUE_TYPES_RET = "MethodNestedGenericOwnValueTypes";
    public const string METHOD_RES_NESTED_GENERIC_PARENT_RET = "MethodNestedGenericParent";

    public const string RES_TYPE_ARRAY_CLASS = "ClassResTypesArrays";
    public const string METHOD_RES_1D_ARRAY = "Method1DArray";
    public const string METHOD_RES_2D_ARRAY = "Method2DArray";
    public const string METHOD_RES_3D_ARRAY = "Method3DArray";
    public const string METHOD_RES_2D_JAGGED_ARRAY = "Method2DJaggedArray";
    public const string METHOD_RES_3D_JAGGED_ARRAY = "Method3DJaggedArray";
    public const string METHOD_RES_GEN_ARR_ENUMSTRING = "MethodGenericStringArr";
    public const string METHOD_RES_GEN_ARR_DYNOBJ = "MethodGenericDynObjArr";
    public const string METHOD_RES_GEN_ARR_OBJDYN = "MethodGenericObjDynArr";
    public const string METHOD_RES_GEN_ARR_INTSTRBOOL = "MethodGenericIntStringBoolArr";
    public const string METHOD_RES_GEN_ARR_COMPLEX_A = "MethodGenericComplexArrAReturn";
    public const string METHOD_RES_GEN_ARR_COMPLEX_B = "MethodGenericComplexArrBReturn";
    public const string METHOD_RES_GEN_ARR_COMPLEX_C = "MethodGenericComplexArrCReturn";
    public const string METHOD_RES_GEN_ARR_COMPLEX_D = "MethodGenericComplexArrDReturn";
    public const string METHOD_RES_GEN_ARR_COMPLEX_E = "MethodGenericComplexArrEReturn";
    public const string METHOD_RES_GEN_ARR_COMPLEX_F = "MethodGenericComplexArrFReturn";
    public const string METHOD_RES_GEN_ARR_COMPLEX_G = "MethodGenericComplexArrGReturn";
    public const string METHOD_RES_GEN_ARR_COMPLEX_H = "MethodGenericComplexArrHReturn";
    
    public const string RES_TYPE_GENERIC_CLASS = "ClassResTypeGenerics";
    public const string METHOD_RES_GEN_ENUMSTRING = "MethodGenericString";
    public const string METHOD_RES_GEN_DYNOBJ = "MethodGenericDynObj";
    public const string METHOD_RES_GEN_OBJDYN = "MethodGenericObjDyn";
    public const string METHOD_RES_GEN_INTSTRBOOL = "MethodGenericIntStringBool";
    public const string METHOD_RES_GEN_COMPLEX_A = "MethodGenericComplexAReturn";
    public const string METHOD_RES_GEN_COMPLEX_B = "MethodGenericComplexBReturn";
    public const string METHOD_RES_GEN_COMPLEX_C = "MethodGenericComplexCReturn";
    public const string METHOD_RES_GEN_COMPLEX_D = "MethodGenericComplexDReturn";
    public const string METHOD_RES_GEN_COMPLEX_E = "MethodGenericComplexEReturn";
    public const string METHOD_RES_GEN_COMPLEX_F = "MethodGenericComplexFReturn";
    public const string METHOD_RES_GEN_COMPLEX_G = "MethodGenericComplexGReturn";
    public const string METHOD_RES_GEN_COMPLEX_H = "MethodGenericComplexHReturn";

    public const string RES_TYPE_TUPLE_CLASS = "ClassResTuples";
    public const string METHOD_RES_TUPLE_ONE = "MethodOneTuple";
    public const string METHOD_RES_TUPLE_TWO = "MethodTwoTuple";
    public const string METHOD_RES_TUPLE_THREE = "MethodThreeTuple";
    public const string METHOD_RES_TUPLE_FOUR = "MethodFourTuple";
    public const string METHOD_RES_VALUE_TUPLE_TWO = "MethodTwoValueTuple";
    public const string METHOD_RES_VALUE_TUPLE_THREE = "MethodThreeValueTuple";
    public const string METHOD_RES_VALUE_TUPLE_FOUR = "MethodFourValueTuple";
    public const string METHOD_RES_VALUE_TUPLE_COMPLEX = "MethodComplexValueTuple";
    public const string METHOD_RES_TUPLE_COMPLEX = "MethodComplexTuple";
    public const string METHOD_RES_ARR_VALUE_TUPLE_COMPLEX = "MethodComplexValueArrTuple";
    public const string METHOD_RES_ARR_TUPLE_COMPLEX = "MethodComplexArrTuple";
    public const string METHOD_RES_ARR_VALUE_TUPLE_COMPLEX2 = "MethodComplexValueArrTuple2";
    public const string METHOD_RES_ARR_TUPLE_COMPLEX2 = "MethodComplexArrTuple2";

    public const string RES_TYPE_GENERIC_VALUE_TYPE_CLASS = "ClassResGenericValueTypes";
    public const string METHOD_RES_GENERIC_VALUE_TYPES = "MethodGenericValueTypes";
    public const string METHOD_RES_GENERIC_OWN_VALUE_TYPES = "MethodGenericOwnValueTypes";
    public const string METHOD_RES_GENERIC_MIXED_VALUE_TYPES = "MethodGenericMixedValueTypes";

    #endregion
  }
}