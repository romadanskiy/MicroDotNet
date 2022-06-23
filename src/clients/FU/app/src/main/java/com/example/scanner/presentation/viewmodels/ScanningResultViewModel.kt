package com.example.scanner.presentation.viewmodels

import android.widget.ImageView
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.scanner.ApiRequestResult
import com.example.scanner.ApiRequestResultSingle
import com.example.scanner.domain.models.Barcode
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.domain.models.GetGarbageInfo
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.usecase.GetGarbageInfoUseCase
import com.example.scanner.domain.usecase.LoadImageUseCase
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class ScanningResultViewModel(val getGarbageInfoUseCase: GetGarbageInfoUseCase, val loadImageUseCase: LoadImageUseCase): LoadedViewModel() {
    private val garbageInfoMutable: MutableLiveData<ApiRequestResultSingle<GetGarbageInfo>> = MutableLiveData()
    var garbageInfo: LiveData<ApiRequestResultSingle<GetGarbageInfo>>
        get() = garbageInfoMutable
        set(value) {}

    fun getGarbageInfoByBarcode(barcode: Barcode){
        viewModelScope.launch(Dispatchers.IO) {
            val requestResult = getGarbageInfoUseCase.execute(barcode);

            val apiRequestResult = ApiRequestResultSingle(requestResult.responseCode,requestResult.success, requestResult.data, requestResult.messages)
            garbageInfoMutable.postValue(apiRequestResult)
            setLoading(false)
        }
    }

    fun loadImage(placeholder: Int, imageView: ImageView){
        if(garbageInfo.value?.data?.image!=null) {
            setLoading(true)
            loadImageUseCase.execute(garbageInfo.value?.data?.image!!, placeholder, imageView)
            setLoading(false)
        }

    }

    override fun clear() {
        garbageInfoMutable.postValue(null)
        super.clear()
    }
}