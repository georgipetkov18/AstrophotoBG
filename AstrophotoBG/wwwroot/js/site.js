const MAIN_YELLOW_COLOR = '#ebb134';
const LIKED_PICTURE_BTN_COLOR = '#ed2553';
var navigation = document.getElementById('navigation');
var galleryCategoriesfilter = document.getElementById('galleryCategoriesFilter');
var galeryOrderByDiv = document.getElementById('galeryOrder');
var galeryOrderBy = document.getElementById('galeryOrderSelect');
var showPictureButton = document.getElementById('pictureInfo');
var deletePictureButton = document.getElementById('adminDeletePicture');
var currentFilter = '';
var pageSize = 24;
var pageIndex = 0;

if (showPictureButton) {
    showPictureButton.addEventListener('click', getPhotoInfo);
}

if (deletePictureButton) {
    deletePictureButton.addEventListener('click', deletePicture);
}

if (galleryCategoriesfilter) {
    galleryCategoriesfilter.addEventListener('change', (e) => {
        let currentOption = e.target.value;
        let categoryName = currentOption;
        loadNewData(categoryName);
        let currentUrl = document.location.href.split('?')[0];
        let pathElements = splitPathName(document.location.pathname);


        if (pathElements.length < 3) {
            history.pushState({}, '', `${currentUrl}/${currentOption}`)
        }

        else if (currentOption === 'all') {
            let lastSlashIndex = currentUrl.lastIndexOf('/');
            let toRemove = currentUrl.slice(lastSlashIndex);
            let url = currentUrl.replace(toRemove, '');
            history.pushState({}, '', url)
        }

        else {
            let lastSlashIndex = currentUrl.lastIndexOf('/');
            let toReplace = currentUrl.slice(lastSlashIndex + 1);
            let url = currentUrl.replace(toReplace, currentOption);
            history.pushState({}, '', url)
        }
        galeryOrderBy.value = 'newest';
    });
}

if (galeryOrderByDiv) {
    galeryOrderByDiv.addEventListener('change', (e) => {
        pageIndex = 0;
        let currentOption = e.target.value;
        let query = window.location.search;
        let currentUrl = document.location.href.split('?')[0];
        let pathElements = splitPathName(document.location.pathname);
        let categoryName = 'all';
        if (pathElements.length >= 3) {
            categoryName = pathElements[pathElements.length - 1].replace('%20', ' ').toLowerCase();
        }

        if (currentOption === 'oldest') {

            GetData(categoryName, true, 'oldest');

            if (query === '') {
                history.pushState({}, '', `${currentUrl}?filt=${currentOption}`)
            }
            currentFilter = 'oldest';
        }

        else {
            history.pushState({}, '', `${currentUrl}`);
            GetData(categoryName, true);
            currentFilter = '';
        }
    });
}

function loadNewData(categoryName) {
    pageIndex = 0;
    GetData(categoryName);
}

$(document).ready(function () {
    if (window.location.pathname === '/') {
        const newDiv = document.createElement('div');
        const nav = document.getElementById('navigation');
        const intro = document.getElementById('intro');
        newDiv.appendChild(nav);
        newDiv.appendChild(intro);
        const nextDiv = document.getElementById('render');
        document.body.insertBefore(newDiv, nextDiv);

        navigation.style.backgroundColor = 'transparent';
        navigation.style.fontSize = '110%';

        window.addEventListener('scroll', () => {

            if (document.documentElement.scrollTop !== 0) {
                navigation.style.backgroundColor = '#201f24';
                navigation.style.fontSize = '100%';
            }
            else {
                navigation.style.backgroundColor = 'transparent';
                navigation.style.fontSize = '110%';
            }
        });
    }
    if (galeryOrderBy && galleryCategoriesfilter) {
        let pathname = window.location.pathname;
        let elements = splitPathName(pathname);
        let categoryName = 'all';

        if (elements.length > 2) {
            categoryName = elements[elements.length - 1].replace('%20', ' ').toLowerCase();
        }

        if (pathname.includes('/Gallery/Pictures') && window.location.search.includes('filt=oldest')) {

            GetData(categoryName, true, 'oldest');
            galeryOrderBy.value = 'oldest';
            galleryCategoriesfilter.value = categoryName;
            currentFilter = 'oldest';
        }

        //else if (pathname.includes('/Gallery/Pictures') && window.location.search === '') {
        //    GetData(categoryName);
        //    galleryCategoriesfilter.value = categoryName;
        //    currentFilter = '';
        //}

        else {
            GetData(categoryName);
            galleryCategoriesfilter.value = categoryName;
            currentFilter = '';
        }
    }
});

window.addEventListener('popstate', (e) => {
    pageIndex = 0;
    let categoryName = 'all';
    let pathElements = splitPathName(document.location.pathname);
    if (pathElements.length >= 3) {
        categoryName = pathElements[pathElements.length - 1].replace('%20', ' ').toLowerCase();
    }

    let query = window.location.search;
    if (query.includes('filt=oldest')) {
        GetData(categoryName, true, 'oldest');
        galeryOrderBy.value = 'oldest';
        galleryCategoriesfilter.value = categoryName;
        currentFilter = 'oldest';
    }
    else {
        GetData(categoryName);
        galeryOrderBy.value = 'newest';
        galleryCategoriesfilter.value = categoryName;
        currentFilter = '';
    }

});

$(window).data('working', false).scroll(function (e) {
    if ($(window).data('working') == true) return;

    if ($(window).scrollTop() ==
        $(document).height() - $(window).height()) {
        let pathName = document.location.pathname;
        let pathElements = splitPathName(pathName);
        if (pathElements.length < 3) {
            GetData('all', false, currentFilter);
        }

        else {
            GetData(pathElements[pathElements.length - 1], false, currentFilter);
        }
    }
});

function GetData(categoryName, emptyPage = true, filter = '') {
    $(window).data('working', true);
    $.ajax({
        type: 'GET',
        url: '/Pictures/GetData',
        data: { "pageIndex": pageIndex, "pageSize": pageSize, "categoryName": categoryName, "filter": filter },
        dataType: 'json',
        success: function (data) {
            if (data != null && data.length !== 0) {
                if (emptyPage) {
                    $("#pictures").empty();
                }
                for (var i = 0; i < data.length; i++) {
                    var source = `/Pictures/DisplayPicture/${data[i].id}`;
                    $("#pictures").append(`
                              <div class="${data[i].category.name} col-md-2 container">
                                    <a href="/Pictures/ById/${data[i].id}">
                                        <figure>
                                            <img class="card-img-top" src="${source}" height="150" alt="${data[i].name}"> 

                                            <figcaption>
                                                <h5>${data[i].name}</h5>
                                                <p>${data[i].category.name}</p>
                                                <p>${data[i].userName}</p>
                                            </figcaption>
                                        </figure>
                                    </a>
                              </div>`);
                }
                pageIndex++;
                $(window).data('working', false);
            }
            else {
                $(window).data('working', true);
            }
        },
        beforeSend: function () {
            $("#progress").show();
        },
        complete: function () {
            $("#progress").hide();
        },
        error: function () {
            alert("Error while retrieving data!");
        }
    });
}

function splitPathName(pathName) {
    let initialPathName = pathName.replace('/', '');
    let pathElements = initialPathName.split('/');
    return pathElements;
}

var likeButton = document.getElementById('pictureLikes');

if (likeButton) {
    likeButton.addEventListener('click', (e) => {
        let pathElements = window.location.pathname.split('/');
        let photoId = pathElements[pathElements.length - 1];
        changeLikes(photoId);
    });
}

async function changeLikes(photoId) {
    const chooseActionUrl = `https://localhost:44382/Pictures/ChooseAction/${photoId}`;
    let status = 'not liked';

    try {
        let response = await fetch(chooseActionUrl);

        if (response.status === 404) {
            window.location.pathname = '/Account/Login';
            return;
        }

        let data = await response.json();
        status = data.status;

    } catch (error) {
        console.log(error);
    }

    let url = `https://localhost:44382/Pictures/IncrementLikes/${photoId}`;

    if (status === 'liked') {
        url = `https://localhost:44382/Pictures/ReduceLikes/${photoId}`;
    }


    const likesContainer = document.getElementById('likes');
    try {
        let response = await fetch(url);

        if (response.status === 404) {
            window.location.pathname = '/Account/Login';
            return;
        }

        let data = await response.json();
        let likes = data.likes;
        likesContainer.innerText = likes;

        if (url.includes('IncrementLikes')) {
            likeButton.style.backgroundColor = LIKED_PICTURE_BTN_COLOR;
            likeButton.style.color = 'white';
            likeButton.innerHTML = '<i class="fa fa-heart" style="color:white" aria-hidden="true"></i> Харесано';
        }
        else {
            likeButton.style.backgroundColor = MAIN_YELLOW_COLOR;
            likeButton.style.color = 'black';
            likeButton.textContent = 'Харесай';
        }
    } catch (error) {
        console.log(error);
    }
}

async function getPhotoInfo() {
    let inputField = document.getElementById('deletePicture');
    let visualizeDiv = document.getElementById('visualizePicture');

    visualizeDiv.innerHTML = '';

    inputField.addEventListener('input', () => {
        showPictureButton.disabled = false;
    });

    let pictureId = inputField.value;
    let url = `/Admin/GetPictureInfo/${pictureId}`;
    let response = await fetch(url);

    if (response.status === 404) {
        window.location.pathname = '/Admin/DeletePicture';
        return;
    }

    let data = await response.json();
    let pictureName = document.createElement('h3');
    pictureName.textContent = data.name;

    let image = document.createElement('img');
    image.height = 250;
    image.width = 500;
    image.src = `/Pictures/DisplayPicture/${pictureId}`;

    let techniqueLabel = document.createElement('h5');
    techniqueLabel.textContent = 'Техника:';
    let techiqueContent = document.createElement('p');
    techiqueContent.textContent = data.technique;

    let descriptionLabel = document.createElement('h5');
    descriptionLabel.textContent = 'Описание:';
    let descriptionContent = document.createElement('p');
    descriptionContent.textContent = data.description;

    visualizeDiv.appendChild(pictureName);
    visualizeDiv.appendChild(image);
    visualizeDiv.appendChild(techniqueLabel);
    visualizeDiv.appendChild(techiqueContent);
    visualizeDiv.appendChild(descriptionLabel);
    visualizeDiv.appendChild(descriptionContent);
    showPictureButton.disabled = true;
}

async function deletePicture() {
    let inputField = document.getElementById('deletePicture');
    let pictureId = inputField.value;
    let url = `/Admin/DelPicture/${pictureId}`;
    let response = await fetch(url);

    if (response.status === 404) {
        window.location.pathname = '/Admin/DeletePicture';
        return;
    }

    let data = await response.json();
    window.location.href = '/Admin/DeletePicture';
}
