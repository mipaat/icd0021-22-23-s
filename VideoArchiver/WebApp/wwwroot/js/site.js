// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const elements = document.getElementsByClassName("date-time-local");
for (const element of elements) {
    element.classList.remove("date-time-local");
    const culture = element.getAttribute("culture");
    element.textContent = (new Date(element.textContent)).toLocaleString(culture);
}

for (const element of document.getElementsByClassName("image-load-container")) {
    const widthStr = element.getAttribute("width");
    const heightStr = element.getAttribute("height");
    const width = widthStr ? parseInt(widthStr) : null;
    const height = heightStr ? parseInt(heightStr) : null;
    const image = new Image(width, height);
    image.alt = element.getAttribute("alt");
    image.src = element.getAttribute("src");
    element.appendChild(image);
}