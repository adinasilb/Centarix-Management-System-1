import React, { Component } from 'react';

const CategoryListSettings = (props) => {

    //constructor(props) {
    //    super(props);
    //    this.state = { categories: this.props.categories };
    //    this.categoryField1 = this.categoryField1.bind(this);
    //    this.categoryField2 = this.categoryField2.bind(this);
    //}
    //{ (this.props.categories.ColumnNumber == 1) ? this.categoryField1 : this.categoryField2 }
    const categoryField1 = (e) => {
        alert("category field 1");
        var element = document.getElementsByClassName("category-list-1")[0].getElementsByClassName("selected")[0];
        element.classList.remove("selected");
        e.target.classList.add("selected");
        var catNum = e.target.getAttribute('data-catid');
        props.changeCategory("ParentCategory", catNum);
    }
    const categoryField2 = (e) => {
        alert("category field 2");
        var element = document.getElementsByClassName("category-list-2")[0].getElementsByClassName("selected")[0];
        element.classList.remove("selected");
        e.target.classList.add("selected");
        var catNum = e.target.getAttribute('data-catid');
        props.changeCategory("ProductSubcategory", catNum);
    }

    return (
        <div className="row lab-man-form">
            <div className={"col-12 category-cols ch-scrollable h-100 " + (props.categories.ColumnNumber)}>
                <input type="text" placeholder="search" className="search-by-category col-input" />
                {props.categories.CategoryBases.map((CategoryBase, i) => (
                    <button key={"Category" + i} href="#" className={"category-field col-input " + " category-field" + props.categories.ColumnNumber + ((i == 0) ? " selected " : "")}
                        data-catid={CategoryBase.ID} onClick={props.categories.ColumnNumber == 1 ? categoryField1 : categoryField2} >
                        {CategoryBase.Description}
                    </button>
                ))
                }
            </div>
        </div>
    )

}

export default CategoryListSettings;