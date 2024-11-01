#pragma once
#define WIN32_LEAN_AND_MEAN

#include <metahost.h>
#pragma comment(lib, "MSCorEE.lib")
#pragma warning( disable:4996 )

#import "mscorlib.tlb" auto_rename
using namespace mscorlib;

bool readBinFile(const char fileName[], char*& bufPtr, DWORD& length) {
    if (FILE* fp = fopen(fileName, "rb")) {
        fseek(fp, 0, SEEK_END);
        length = ftell(fp);
        bufPtr = new char[length + 1];
        fseek(fp, 0, SEEK_SET);
        fread(bufPtr, sizeof(char), length, fp);
        return true;
    }
    else return false;
}

ICorRuntimeHost* getCorRtHost_byVersion(LPCWSTR sz_runtimeVersion) {
    ICLRRuntimeInfo* pRuntimeInfo = NULL;
    ICorRuntimeHost* pRuntimeHost = NULL;
    ICLRMetaHost* pMetaHost = NULL;

    CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (VOID**)&pMetaHost);
    pMetaHost->GetRuntime(sz_runtimeVersion, IID_ICLRRuntimeInfo, (VOID**)&pRuntimeInfo);
    pRuntimeInfo->GetInterface(CLSID_CorRuntimeHost, IID_ICorRuntimeHost, (VOID**)&pRuntimeHost);
    pRuntimeHost->Start();

    return pRuntimeHost;
}

_AppDomainPtr getDefaultDomain(ICorRuntimeHost* pRuntimeHost) {
    IUnknownPtr pAppDomainThunk = NULL;
    _AppDomainPtr pDefaultAppDomain = NULL;

    pRuntimeHost->GetDefaultDomain(&pAppDomainThunk);
    pAppDomainThunk->QueryInterface(__uuidof(_AppDomain), (LPVOID*)&pDefaultAppDomain);
    return pDefaultAppDomain;
}

_AssemblyPtr getAssembly_fromBinary(_AppDomainPtr pDefaultAppDomain, LPBYTE rawData, ULONG lenRawData) {
    _AssemblyPtr pAssembly = NULL;
    SAFEARRAY* pSafeArray = SafeArrayCreate(VT_UI1, 1, new SAFEARRAYBOUND{ lenRawData , 0 });

    void* pvData = NULL;

    SafeArrayAccessData(pSafeArray, &pvData);
    SafeArrayUnaccessData(pSafeArray);
    memcpy(pvData, rawData, lenRawData);
    pDefaultAppDomain->raw_Load_3(pSafeArray, &pAssembly);


    return pAssembly;
}

SAFEARRAY* newArguments(int argc, wchar_t** argv) {
    VARIANT args;
    args.vt = VT_ARRAY | VT_BSTR;
    args.parray = SafeArrayCreate(VT_BSTR, 1, new SAFEARRAYBOUND{ ULONG(argc) , 0 });
    for (int i = 0; i < argc; i++) SafeArrayPutElement(args.parray, (LONG*)&i, SysAllocString(argv[i]));

    SAFEARRAY* params = SafeArrayCreate(VT_VARIANT, 1, new SAFEARRAYBOUND{ 1, 0 });

    LONG indx = 0;
    SafeArrayPutElement(params, &indx, &args);
    return params;
}

ICorRuntimeHost* bruteforce_CLRhost() {
    ICLRMetaHost* metaHost = NULL;
    IEnumUnknown* runtime = NULL;
    ICLRRuntimeInfo* runtimeInfo = nullptr;
    DWORD bytes;
    if (CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&metaHost) != S_OK) {
        return NULL;
    }

    if (metaHost->EnumerateInstalledRuntimes(&runtime) != S_OK) 
    {
        return NULL;
    }
    auto frameworkName = (LPWSTR)LocalAlloc(LPTR, 2048);
    IUnknown* enumRuntime = nullptr;
    // Enumerate through runtimes and show supported frameworks
    while (runtime->Next(1, &enumRuntime, 0) == S_OK) {
        if (enumRuntime->QueryInterface<ICLRRuntimeInfo>(&runtimeInfo) == S_OK) {
            if (runtimeInfo != NULL) {
                runtimeInfo->GetVersionString(frameworkName, &bytes);
            }
        }
    }
    return getCorRtHost_byVersion(frameworkName);
}


int Test()
{
    const char* path = "mods/DE Library/DELibrary.NET.dll";

    PCHAR ptrBinary = 0;
    DWORD lenBinary = 1337;
    
    //Change exe path to yours
    //I made this an absolute path because i didnt want to copy files around for each change to my own code tests
    if (!readBinFile(path, ptrBinary, lenBinary))
        return 0;

    ICorRuntimeHost* pRuntimeHost = getCorRtHost_byVersion(L"v4.0.30319");

    if (!pRuntimeHost)
    {
        ICorRuntimeHost* pRuntimeHost = getCorRtHost_byVersion(L"v4.0");
    }

    if (!pRuntimeHost) 
        if ((pRuntimeHost = bruteforce_CLRhost()) == 0)
        return -1;

    _MethodInfoPtr pMethodInfo = NULL;
    // fetch the default domain
    if (auto pDefaultAppDomain = getDefaultDomain(pRuntimeHost))
        // load .net module into CLR (PE binary)
        if (_AssemblyPtr pAssembly = getAssembly_fromBinary(pDefaultAppDomain, LPBYTE(ptrBinary), (lenBinary)))
            //A ssembly.EntryPoint Property
            if (FAILED(pAssembly->get_EntryPoint(&pMethodInfo))) 
                return -1;


    VARIANT var = VARIANT();

    if (HRESULT hr = pMethodInfo->raw_Invoke_3(VARIANT(), newArguments(0, 0), &var) < 0) 
        return -1;
    

    return 0;
}
