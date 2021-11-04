

typedef void (*BaseOnLineEvent)(long BaseID, long BaseStates);




class CConnectionEventListener :public IBaseConnectionEvents
 
{
 
public:
 
    CConnectionEventListener(void);
 
    ~CConnectionEventListener(void);
	

private:
 
    DWORD  m_dwRefCount;
    static BaseOnLineEvent baseOnLineEvent;
public:
	static void init(BaseOnLineEvent baseOnLineEvent);

 
    HRESULT STDMETHODCALLTYPE QueryInterface(REFIID iid, void **ppvObject)
 
    {
 
       if (iid == IID_IBaseConnectionEvents)
 
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
 
       switch(dispIdMember) // Implement a different callback function according different dispIdMember(event method id )
 
       {
 
       case 201:  //[id(0x000000c9)]
 
           {
 
              // 1st param : [in] long lValue.
 
              VARIANT varlValue;
 
              long lBaseID = 0;
			  long lBaseState=0;
 
              VariantInit(&varlValue);
 
              VariantClear(&varlValue);
 
              varlValue = (pDispParams->rgvarg)[1];
 
              lBaseID = V_I4(&varlValue);

			  varlValue = (pDispParams->rgvarg)[0];
			  lBaseState=V_I4(&varlValue);
			  baseOnLineEvent(lBaseID,lBaseState);
           }
 
           break;
 
       default:   break;
 
       }
 
 
 
       return S_OK;
 
    }
 
};


