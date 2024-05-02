// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <iostream>
#include "DotNetTest.h"

DWORD WINAPI NetThread(HMODULE hModule)
{
    FILE* f = nullptr;
  
    bool createConsole = GetPrivateProfileIntW(L"DELib", L"EnableConsole", 0, L"mods/DE Library/config.ini");

    //Create Console
    if (createConsole)
    {
        AllocConsole();
        freopen_s(&f, "CONOUT$", "w", stdout);
    }

#if DEBUG
    std::cout << "Starting in 5 seconds" << std::endl;
    Sleep(5000);
#endif

    Test();

    std::cout << "Loader Finished" << std::endl;

    while (true)
    {
        Sleep(1);
    }

    if (createConsole)
    {
        fclose(f);
        FreeConsole();
    }
    FreeLibraryAndExitThread(hModule, 0);
    

    return 0;
}


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        CloseHandle(CreateThread(nullptr, 0, (LPTHREAD_START_ROUTINE)NetThread, hModule, 0, nullptr));
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

