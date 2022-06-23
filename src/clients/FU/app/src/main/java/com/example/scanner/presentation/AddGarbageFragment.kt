package com.example.scanner.presentation

import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.ImageView
import android.widget.Toast
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.scanner.ApiFailedRequestResult
import com.example.scanner.ApiRequestResult
import com.example.scanner.R
import com.example.scanner.databinding.FragmentAddGarbageBinding
import com.example.scanner.databinding.FragmentScanningResultBinding
import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.presentation.models.GarbageCategoryModel
import com.example.scanner.presentation.viewmodels.*
import org.koin.androidx.viewmodel.ext.android.sharedViewModel

class AddGarbageFragment : Fragment() {

    private val addGarbageViewModel by sharedViewModel<AddGarbageViewModel>()
    private val errorViewModel by sharedViewModel<ErrorViewModel>()
    private val scannerViewModel by sharedViewModel<ScannerViewModel>()
    private val garbageCategoriesViewModel by sharedViewModel<GarbageCategoriesViewModel>()
    private lateinit var binding: FragmentAddGarbageBinding

    private val selectImageCode = 100000001
    private val requestCodeExternalStoragePermission = 1002

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentAddGarbageBinding.inflate(inflater, container, false);
        binding.setGarbageImage.setOnClickListener {
            if (ContextCompat.checkSelfPermission(
                    this.requireContext(), android.Manifest.permission.WRITE_EXTERNAL_STORAGE
                ) != PackageManager.PERMISSION_GRANTED
            ) {
                askForExternalStoragePermission()
            } else {
                openGalleryForImage()
                saveDataToViewModel()
            }
        }

        binding.saveGarbage.setOnClickListener {
            addGarbageViewModel.setImage(
                R.drawable.garbage_not_found,
                binding.imageAddGarbage,
                requireContext()
            )
            saveDataToViewModel()
            val info = addGarbageViewModel.garbageInfo.value
            if (info != null)
                addGarbageViewModel.addGarbageInfo()
        }

        binding.setGarbageCategories.setOnClickListener {
            saveDataToViewModel()
            var supportFragmentManager = activity?.supportFragmentManager;
            supportFragmentManager?.beginTransaction()
                ?.replace(R.id.fragment_container, GarbageCategoriesFragment())?.commit();
        }

        val info =
            Observer<GarbageInfo> { info ->

                if (info != null) {
                    binding.nameEdit.setText(info.name)
                    binding.descriptionEdit.setText(info.description)
                    addGarbageViewModel.loadImage(
                        addGarbageViewModel.imagePath.value,
                        R.drawable.garbage_not_found,
                        binding.imageAddGarbage,
                    )
                }
            }

        val result =
            Observer<ApiRequestResult<String>> { result ->
                if (result != null) {
                    if (result.success) {
                        var supportFragmentManager = activity?.supportFragmentManager;
                        supportFragmentManager?.beginTransaction()
                            ?.replace(R.id.fragment_container, ScanningResultFragment())?.commit();
                        addGarbageViewModel.setLoading(false)
                    } else {

                        errorViewModel.handleRequestError(
                            ApiFailedRequestResult(
                                result.code,
                                result.errorMessages
                            ), AddGarbageFragment()
                        )
                        addGarbageViewModel.setResult(null)
                        addGarbageViewModel.setLoading(false)
                        val supportFragmentManager = activity?.supportFragmentManager
                        supportFragmentManager?.beginTransaction()
                            ?.replace(com.example.scanner.R.id.fragment_container, ErrorFragment())
                            ?.commit();
                    }
                }
            }

        val loading =
            Observer<Boolean> { loading ->
                if (loading) {
                    binding.addGarbageLoader.visibility = View.VISIBLE
                    showFields(false)
                } else {
                    binding.addGarbageLoader.visibility = View.GONE
                    showFields(true)
                }
            }

        addGarbageViewModel.garbageInfo.observe(viewLifecycleOwner, info)
        addGarbageViewModel.result.observe(viewLifecycleOwner, result)
        addGarbageViewModel.loading.observe(viewLifecycleOwner, loading)
        return binding.root;
    }

    private fun saveDataToViewModel() {
        var categories = emptyList<GarbageCategory>()
        if (garbageCategoriesViewModel.categoryIds.value != null) {
            categories = List(garbageCategoriesViewModel.categoryIds.value!!.size) {
                GarbageCategory("", garbageCategoriesViewModel.categoryIds.value!![it])
            }
        }

        val garbageInfo = GarbageInfo(
            binding.nameEdit.text.toString(),
            binding.descriptionEdit.text.toString(),
            categories,
            scannerViewModel.barcode.value!!.barcode,
            addGarbageViewModel.image.value
        )
        addGarbageViewModel.setGarbageInfo(garbageInfo)
    }

    fun openGalleryForImage() {
        val intent = Intent(Intent.ACTION_PICK)
        intent.type = "image/*"
        startActivityForResult(intent, selectImageCode)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (resultCode == Activity.RESULT_OK && requestCode == selectImageCode) {
            if (data != null) {
                val imageView = binding.imageAddGarbage;
                addGarbageViewModel.loadImage(
                    data.data!!.toString(),
                    R.drawable.garbage_not_found,
                    imageView,
                )
            }
        }
    }

    private fun askForExternalStoragePermission() {
        ActivityCompat.requestPermissions(
            this.requireActivity(),
            arrayOf(
                android.Manifest.permission.WRITE_EXTERNAL_STORAGE
            ),
            requestCodeExternalStoragePermission
        )
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == requestCodeExternalStoragePermission && grantResults.isNotEmpty()) {
            if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                openGalleryForImage()
                saveDataToViewModel()
            } else {
                Toast.makeText(
                    activity?.applicationContext,
                    "Permission Denied",
                    Toast.LENGTH_SHORT
                ).show()
            }
        }
    }

    private fun showFields(show: Boolean) {
        var visibility = View.GONE
        if (show) {
            visibility = View.VISIBLE
        }
        binding.setGarbageCategories.visibility = visibility
        binding.imageAddGarbage.visibility = visibility
        binding.nameText.visibility = visibility
        binding.nameEdit.visibility = visibility
        binding.descriptionText.visibility = visibility
        binding.descriptionEdit.visibility = visibility
        binding.setGarbageCategories.visibility = visibility
        binding.setGarbageImage.visibility = visibility
        binding.saveGarbage.visibility = visibility
    }
}