// dllmain.cpp : Defines the entry point for the DLL application.
#define WIN32_LEAN_AND_MEAN
#include "pch.h"
#include "DotNetTest.h"
#include <math.h>

DWORD WINAPI NetThread(HMODULE hModule)
{
    int i = 0;
    float myfloat = 3.17500;
    const char* weber = "myweber";

    for (int i = 0; i < 363; i++)
    {
        cos(myfloat);
        i++;
    }

    i = 0;

    printf("Starting DE Library.NET");
    Sleep(1000);
    Test();
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

