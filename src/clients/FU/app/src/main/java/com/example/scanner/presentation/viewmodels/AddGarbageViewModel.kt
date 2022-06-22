package com.example.scanner.presentation.viewmodels

import android.content.Context
import android.content.Intent.*
import android.graphics.Bitmap
import android.graphics.drawable.BitmapDrawable
import android.widget.ImageView
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.viewModelScope
import com.example.scanner.ApiRequestResult
import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.usecase.AddGarbageInfoUseCase
import com.example.scanner.domain.usecase.GetGarbageCategoriesUseCase
import com.example.scanner.domain.usecase.LoadImageUseCase
import com.example.scanner.presentation.utils.BitmapToFileConverter
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.io.File

class AddGarbageViewModel(
    val addGarbageInfoUseCase: AddGarbageInfoUseCase,
    val loadImageUseCase: LoadImageUseCase,
) : LoadedViewModel() {
    private val mutableGarbageInfo: MutableLiveData<GarbageInfo> = MutableLiveData()
    private val mutableImage: MutableLiveData<File> = MutableLiveData()
    private val mutableCategories: MutableLiveData<List<GarbageCategory>> = MutableLiveData()
    private val mutableImagePath: MutableLiveData<String> = MutableLiveData()
    private val mutableResult: MutableLiveData<ApiRequestResult<String>> = MutableLiveData()
    val garbageInfo: LiveData<GarbageInfo> = mutableGarbageInfo
    val image: LiveData<File> = mutableImage
    val categories: LiveData<List<GarbageCategory>> = mutableCategories
    val imagePath: LiveData<String> = mutableImagePath
    val result: LiveData<ApiRequestResult<String>> = mutableResult

    fun addGarbageInfo(){
        viewModelScope.launch(Dispatchers.IO) {
            loadingMutable.postValue(true)
            val requestResult = addGarbageInfoUseCase.execute(mutableGarbageInfo.value!!);

            val apiRequestResult = ApiRequestResult(
                requestResult.responseCode,
                requestResult.success,
                requestResult.data,
                requestResult.messages
            )
            mutableResult.postValue(apiRequestResult)
        }
    }

    fun setGarbageInfo(garbageInfo: GarbageInfo){
        mutableGarbageInfo.value = garbageInfo
    }

    fun setImage(placeholder: Int, imageView: ImageView, context: Context) {
        val image = mutableImagePath?.value
        if(image!=null) {
            loadImage(image,placeholder,imageView)
            var bitmap = (imageView.drawable as BitmapDrawable).bitmap
            val bitmapToFileConverter = BitmapToFileConverter()
            val file = bitmapToFileConverter.bitmapToFile(
                context,
                bitmap,
                "garbageAddImage" + garbageInfo.value?.barcode
            )
            mutableImage.value = file
        }
    }

    fun loadImage(imageUrl: String?, placeholder: Int, imageView: ImageView){
        if(imageUrl != null) {
            loadImageUseCase.execute(imageUrl, placeholder, imageView)
            mutableImagePath.value = imageUrl
        }
    }

    fun categories(categories: List<GarbageCategory>) {
        mutableCategories.postValue(categories)
    }

    fun setResult(result: ApiRequestResult<String>?){
        mutableResult.postValue(result)
    }

    override fun clear() {
        super.clear()
        loadingMutable.postValue(false)
        mutableCategories.postValue(null);
        mutableImage.postValue(null);
        mutableGarbageInfo.postValue(null);
        mutableImagePath.postValue(null)
        mutableResult.postValue(null)
    }
}