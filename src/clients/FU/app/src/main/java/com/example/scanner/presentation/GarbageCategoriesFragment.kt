package com.example.scanner.presentation

import android.R
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView.OnItemClickListener
import android.widget.ArrayAdapter
import android.widget.CheckedTextView
import android.widget.ListView
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import com.example.scanner.ApiFailedRequestResult
import com.example.scanner.databinding.FragmentGarbageCategoriesListBinding
import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.presentation.models.GarbageCategoryModel
import com.example.scanner.presentation.viewmodels.ErrorViewModel
import com.example.scanner.presentation.viewmodels.GarbageCategoriesViewModel
import org.koin.androidx.viewmodel.ext.android.sharedViewModel


class GarbageCategoriesFragment : Fragment() {

    private val garbageCategoriesViewModel by sharedViewModel<GarbageCategoriesViewModel>()
    private val errorViewModel by sharedViewModel<ErrorViewModel>()
    private lateinit var binding: FragmentGarbageCategoriesListBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentGarbageCategoriesListBinding.inflate(inflater, container, false);

        binding.garbageCategoriesList.setChoiceMode(ListView.CHOICE_MODE_MULTIPLE)

        binding.garbageCategoriesList.setOnItemClickListener(OnItemClickListener { parent, view, position, id ->
            val v = view as CheckedTextView
            val category: GarbageCategoryModel =
                binding.garbageCategoriesList.getItemAtPosition(position) as GarbageCategoryModel
            val currentSelected =
                garbageCategoriesViewModel.categoryIds.value!!.contains(category.id)
            if (currentSelected) {
                garbageCategoriesViewModel.removeSelectedCategory(category.id)
                category.setSelected(false)
            } else {
                garbageCategoriesViewModel.addSelectedCategory(category.id)
                category.setSelected(true)
            }

        })

        val loading =
            Observer<Boolean> { loading ->
                if (loading) {
                    binding.garbageCategoriesList.visibility = View.INVISIBLE
                    binding.progressBarCategories.visibility = View.VISIBLE
                } else {
                    binding.progressBarCategories.visibility = View.INVISIBLE
                    binding.garbageCategoriesList.visibility = View.VISIBLE
                }
            }

        val requestResult =
            Observer<RequestResult<GarbageCategory>> { result ->
                if (result != null && result.success) {
                    if (garbageCategoriesViewModel.categoriesUI.value == null || garbageCategoriesViewModel.categoriesUI.value!!.size == 0) {
                        garbageCategoriesViewModel.setCategories(result.data!!)
                    }
                } else if (result != null) {
                    errorViewModel.handleRequestError(
                        ApiFailedRequestResult(
                            result.responseCode,
                            result.messages
                        ), null
                    )
                    val supportFragmentManager = activity?.supportFragmentManager
                    supportFragmentManager?.beginTransaction()
                        ?.replace(com.example.scanner.R.id.fragment_container, ErrorFragment())
                        ?.commit();
                }
            }

        val categories =
            Observer<List<GarbageCategoryModel>> { category ->
                val arrayAdapter = activity?.let {
                    ArrayAdapter<GarbageCategoryModel>(
                        it,
                        R.layout.simple_list_item_checked,
                        category
                    )
                }

                binding.garbageCategoriesList.setAdapter(arrayAdapter)

                if (category != null) {
                    for (i in 0 until category.size!!) {
                        val currentCategory = category.get(i)
                        val ids = garbageCategoriesViewModel.categoryIds.value!!
                        val selected =
                            garbageCategoriesViewModel.categoryIds.value!!.contains(currentCategory.id)
                        currentCategory.setSelected(selected)
                        binding.garbageCategoriesList.setItemChecked(i, selected)
                    }
                }
            }

        garbageCategoriesViewModel.categoriesResult.observe(viewLifecycleOwner, requestResult)
        garbageCategoriesViewModel.loading.observe(viewLifecycleOwner, loading)
        garbageCategoriesViewModel.categoriesUI.observe(viewLifecycleOwner, categories)

        if (garbageCategoriesViewModel.categoriesResult.value == null) {
            garbageCategoriesViewModel.getCategories()
        }

        return binding.root;
    }
}