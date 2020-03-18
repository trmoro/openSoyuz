#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace SKW
{
    template<class T>
    public ref class SKWObject
    {
    protected:
        T* m_Instance;
    public:

        //Constructor
        SKWObject(T* instance) : m_Instance(instance)
        {
        }

        //Destructor
        virtual ~SKWObject()
        {
            if (m_Instance != nullptr)
                delete m_Instance;
        }

        //Finalizer called by garbage collector
        !SKWObject()
        {
            if (m_Instance != nullptr)
                delete m_Instance;
        }

        //Get
        T* GetInstance()
        {
            return m_Instance;
        }

        //String to Char Array
        static const char* stringToCharArray(String^ string)
        {
            const char* str = (const char*)(Marshal::StringToHGlobalAnsi(string)).ToPointer();
            return str;
        }
    };
}