#pragma once

#include <stdio.h>
#include <windows.h>
#include <mscoree.h>
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
    BOOL bLoadable;

    /* Get ICLRMetaHost instance */
    if (FAILED(CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (VOID**)&pMetaHost)))
    {
        printf("[!] CLRCreateInstance(...) failed\n");
        return NULL;
    }
    else printf("[+] CLRCreateInstance(...) succeeded\n");

    /* Get ICLRRuntimeInfo instance */
    if (FAILED(pMetaHost->GetRuntime(sz_runtimeVersion, IID_ICLRRuntimeInfo, (VOID**)&pRuntimeInfo))) {
        printf("[!] pMetaHost->GetRuntime(...) failed\n");
        return NULL;
    }
    else printf("[+] pMetaHost->GetRuntime(...) succeeded\n");

    /* Check if the specified runtime can be loaded */
    if (FAILED(pRuntimeInfo->IsLoadable(&bLoadable)) || !bLoadable) {
        printf("[!] pRuntimeInfo->IsLoadable(...) failed\n");
        return NULL;
    }
    else printf("[+] pRuntimeInfo->IsLoadable(...) succeeded\n");

    /* Get ICorRuntimeHost instance */
    if (FAILED(pRuntimeInfo->GetInterface(CLSID_CorRuntimeHost, IID_ICorRuntimeHost, (VOID**)&pRuntimeHost))) {
        printf("[!] pRuntimeInfo->GetInterface(...) failed\n");
        return NULL;
    }
    else printf("[+] pRuntimeInfo->GetInterface(...) succeeded\n");

    /* Start the CLR */
    if (FAILED(pRuntimeHost->Start())) {
        printf("[!] pRuntimeHost->Start() failed\n");
        return NULL;
    }
    else printf("[+] pRuntimeHost->Start() succeeded\n");
    return pRuntimeHost;
}

_AppDomainPtr getDefaultDomain(ICorRuntimeHost* pRuntimeHost) {
    IUnknownPtr pAppDomainThunk = NULL;
    if (FAILED(pRuntimeHost->GetDefaultDomain(&pAppDomainThunk))) {
        printf("[!] pRuntimeHost->GetDefaultDomain(...) failed\n");
        return NULL;
    }
    else printf("[+] pRuntimeHost->GetDefaultDomain(...) succeeded\n");

    /* Equivalent of System.AppDomain.CurrentDomain in C# */
    _AppDomainPtr pDefaultAppDomain = NULL;
    if (FAILED(pAppDomainThunk->QueryInterface(__uuidof(_AppDomain), (LPVOID*)&pDefaultAppDomain))) {
        printf("[!] pAppDomainThunk->QueryInterface(...) failed\n");
        return NULL;
    }
    else printf("[+] pAppDomainThunk->QueryInterface(...) succeeded\n");
    return pDefaultAppDomain;
}

_AssemblyPtr getAssembly_fromBinary(_AppDomainPtr pDefaultAppDomain, LPBYTE rawData, ULONG lenRawData) {
    _AssemblyPtr pAssembly = NULL;
    SAFEARRAY* pSafeArray = SafeArrayCreate(VT_UI1, 1, new SAFEARRAYBOUND{ lenRawData , 0 });

    void* pvData = NULL;
    if (FAILED(SafeArrayAccessData(pSafeArray, &pvData))) {
        printf("[!] SafeArrayAccessData(...) failed\n");
        return -1;
    }
    else printf("[+] SafeArrayAccessData(...) succeeded\n");

    memcpy(pvData, rawData, lenRawData);
    if (FAILED(SafeArrayUnaccessData(pSafeArray))) {
        printf("[!] SafeArrayUnaccessData(...) failed\n");
        return NULL;
    }
    else printf("[+] SafeArrayUnaccessData(...) succeeded\n");

    /* Equivalent of System.AppDomain.CurrentDomain.Load(byte[] rawAssembly) */
    if (FAILED(pDefaultAppDomain->raw_Load_3(pSafeArray, &pAssembly))) {
        printf("[!] pDefaultAppDomain->Load_3(...) failed\n");
        return NULL;
    }
    else printf("[+] pDefaultAppDomain->Load_3(...) succeeded\n");
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
        printf("[x] Error: CLRCreateInstance(..)\n");
        return NULL;
    }

    if (metaHost->EnumerateInstalledRuntimes(&runtime) != S_OK) {
        printf("[x] Error: EnumerateInstalledRuntimes(..)\n");
        return NULL;
    }
    auto frameworkName = (LPWSTR)LocalAlloc(LPTR, 2048);
    IUnknown* enumRuntime = nullptr;
    // Enumerate through runtimes and show supported frameworks
    while (runtime->Next(1, &enumRuntime, 0) == S_OK) {
        if (enumRuntime->QueryInterface<ICLRRuntimeInfo>(&runtimeInfo) == S_OK) {
            if (runtimeInfo != NULL) {
                runtimeInfo->GetVersionString(frameworkName, &bytes);
                wprintf(L"[*] Supported Framework: %s\n", frameworkName);

            }
        }
    }
    wprintf(L"[*] Current Used Framework: %s\n", frameworkName);
    return getCorRtHost_byVersion(frameworkName);
}


int Test()
{
    PCHAR ptrBinary; DWORD lenBinary;


    //Change exe path to yours
    //I made this an absolute path because i didnt want to copy files around for each change to my own code tests
    if (!readBinFile("DELibrary.NET.dll", ptrBinary, lenBinary))
        return -1;

    printf(" --- Try to Fetch .NET Framework v4.6.1 ---\n");
    ICorRuntimeHost* pRuntimeHost = getCorRtHost_byVersion(L"v4.0.30319");

    if (!pRuntimeHost)
    {
        printf(" --- Fetching v4.6.1 failed, trying to fetch v4.0 ---\n");
        ICorRuntimeHost* pRuntimeHost = getCorRtHost_byVersion(L"v4.0");
    }

    std::cout << "\n Is RuntimeHost null: " << pRuntimeHost << std::endl;

    printf("\n --- Enumerate Available CLR Runtime ---\n");
    if (!pRuntimeHost) if ((pRuntimeHost = bruteforce_CLRhost()) == 0)
        return -1;

    printf("\n --- Execute .NET Module ---\n");
    _MethodInfoPtr pMethodInfo = NULL;
    // fetch the default domain
    if (auto pDefaultAppDomain = getDefaultDomain(pRuntimeHost))
        // load .net module into CLR (PE binary)
        if (_AssemblyPtr pAssembly = getAssembly_fromBinary(pDefaultAppDomain, LPBYTE(ptrBinary), (lenBinary)))
            //A ssembly.EntryPoint Property
            if (FAILED(pAssembly->get_EntryPoint(&pMethodInfo))) {
                printf("[!] pAssembly->get_EntryPoint(...) failed\n");
                return -1;
            }
            else printf("[+] pAssembly->get_EntryPoint(...) succeeded\n");


    VARIANT var = VARIANT();

    /* EntryPoint.Invoke(new string[] { argv_1, argv_2, argv_3, ... } ) */
    if (HRESULT hr = pMethodInfo->raw_Invoke_3(VARIANT(), newArguments(0, 0), &var) < 0) {
        printf("[!] pMethodInfo->Invoke_3(...) failed, hr = %X\n", hr);
        return -1;
    }
    else printf("[+] pMethodInfo->Invoke_3(...) succeeded\n");
    return 0;
}