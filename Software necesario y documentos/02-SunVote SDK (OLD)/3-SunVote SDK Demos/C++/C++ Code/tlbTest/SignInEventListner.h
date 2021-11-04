#include "comutil.h"
#pragma once
typedef void (*KeyStatusEvent)(BSTR BaseTag, 
                        long KeyID, 
                        long ValueType, 
                        BSTR  KeyValue);

class SignInEventListner : public ISignInEvents
{
public:
	SignInEventListner(void);
	~SignInEventListner(void);

	private:
 
    DWORD  m_dwRefCount;
    static KeyStatusEvent keyStatusEvent;
public:
	static void init(KeyStatusEvent keyStatusEvent);


    HRESULT STDMETHODCALLTYPE QueryInterface(REFIID iid, void **ppvObject)
 
    {
 
       if (iid == IID_ISingInEvents)
 
       {
 
           m_dwRefCount++;
 
           *ppvObject = (void *)this;
 
           return S_OK;
 
       }
 
 
 
       if (iid == IID_IUnknown)
 
       {
 
           m_dwRefCount++;            

           *ppvObject = (void *)this;
 
           return S_OK;
 
       }



 
 
 
       return E_NOINTERFACE;
 
    }
 
    ULONG STDMETHODCALLTYPE AddRef()
 
    {
 
       m_dwRefCount++;
 
       return m_dwRefCount;
 
    }
 
 
 
    ULONG STDMETHODCALLTYPE Release()
 
    {
 
       ULONG l;
 
 
 
       l  = m_dwRefCount--;
 
 
 
       if ( 0 == m_dwRefCount)
 
       {
 
           delete this;
 
       }
 
 
 
       return l;
 
    }
 
 
 
    HRESULT STDMETHODCALLTYPE GetTypeInfoCount( 

       /* [out] */ __RPC__out UINT *pctinfo)
 
    {
 
       return S_OK;
 
    }
 
 
 
    HRESULT STDMETHODCALLTYPE GetTypeInfo( 

       /* [in] */ UINT iTInfo,
 
       /* [in] */ LCID lcid,
 
       /* [out] */ __RPC__deref_out_opt ITypeInfo **ppTInfo)
 
    {
 
       return S_OK;
 
    }
 
 
 
     HRESULT STDMETHODCALLTYPE GetIDsOfNames( 

       /* [in] */ __RPC__in REFIID riid,
 
       /* [size_is][in] */ __RPC__in_ecount_full(cNames) LPOLESTR *rgszNames,
 
       /* [range][in] */ UINT cNames,
 
       /* [in] */ LCID lcid,
 
       /* [size_is][out] */ __RPC__out_ecount_full(cNames) DISPID *rgDispId)
 
    {
 
       return S_OK;
 
    }
 
 
 
     /* [local] */ HRESULT STDMETHODCALLTYPE Invoke( 

       /* [in] */ DISPID dispIdMember,
 
       /* [in] */ REFIID riid,
 
       /* [in] */ LCID lcid,
 
       /* [in] */ WORD wFlags,
 
       /* [out][in] */ DISPPARAMS *pDispParams,
 
       /* [out] */ VARIANT *pVarResult,
 
       /* [out] */ EXCEPINFO *pExcepInfo,
 
       /* [out] */ UINT *puArgErr)
 
    {
 
       switch(dispIdMember) // // Implement a different callback function according different dispIdMember(event method id )
 
       {
 
       case 201:   //[id(0x000000c9)]
           {
 
              // 1st param : [in] long lValue.
 
              VARIANT varlValue;
 
              long lKeyID = 0;
			  long lValueType=0;
			  BSTR sBaseTag= (BSTR)"";
              BSTR  sKeyValue= (BSTR)"";
              VariantInit(&varlValue);
 
              VariantClear(&varlValue);

			  varlValue = (pDispParams->rgvarg)[3];
              sBaseTag= V_BSTR(&varlValue); 

              varlValue = (pDispParams->rgvarg)[2];
              lKeyID = V_I4(&varlValue);

			  varlValue = (pDispParams->rgvarg)[1];
			  lValueType=V_I4(&varlValue);

			  varlValue = (pDispParams->rgvarg)[0];
              sKeyValue= V_BSTR(&varlValue); 

              keyStatusEvent(sBaseTag,lKeyID,lValueType,sKeyValue);
 
           }
 
           break;
 
       default:   break;
 
       }
 
 
 
       return S_OK;
 
    }
};

