const camps = [
  {
    id: 1,
    name: "홍천 리버포레 캠핑장",
    address: "강원 홍천군 서면 한치골길 112",
    tel: "033-555-1288",
    region: "강원",
    type: "오토캠핑",
    facilityInfo: "개별 화로대, 전기, 샤워실, 매점, 계곡 산책로",
    imageUrl: "assets/camp-lake.png",
    homepage: "https://example.com/riverfore",
    petAllowed: true,
    totalSiteCount: 42,
    distanceText: "서울 기준 약 1시간 40분",
    rating: 4.8,
  },
  {
    id: 2,
    name: "가평 숲결 글램핑",
    address: "경기 가평군 북면 백둔로 64",
    tel: "031-777-0921",
    region: "경기",
    type: "글램핑",
    facilityInfo: "침구 제공, 바비큐 데크, 공용 주방, 온수 샤워",
    imageUrl: "assets/camp-feed.png",
    homepage: "https://example.com/forestglam",
    petAllowed: false,
    totalSiteCount: 18,
    distanceText: "서울 기준 약 1시간 20분",
    rating: 4.7,
  },
  {
    id: 3,
    name: "태안 솔바람 오토캠프",
    address: "충남 태안군 남면 몽산포길 203",
    tel: "041-333-9012",
    region: "충남",
    type: "차박",
    facilityInfo: "해변 5분, 전기, 개수대, 장작 판매, 일몰 전망",
    imageUrl: "assets/camp-gear.png",
    homepage: "https://example.com/solbaram",
    petAllowed: true,
    totalSiteCount: 55,
    distanceText: "서울 기준 약 2시간 15분",
    rating: 4.6,
  },
  {
    id: 4,
    name: "제주 오름 백패킹 스팟",
    address: "제주 제주시 구좌읍 송당리 88",
    tel: "064-444-3700",
    region: "제주",
    type: "백패킹",
    facilityInfo: "간이 화장실, 전망 데크, 물 보급 지점, 별 관측",
    imageUrl: "assets/camp-forest.png",
    homepage: "https://example.com/oreum",
    petAllowed: false,
    totalSiteCount: 12,
    distanceText: "제주공항 기준 약 45분",
    rating: 4.9,
  },
];

const state = {
  view: "search",
  filtered: [...camps],
  favorites: new Set(),
  compare: new Set([1, 3]),
  loggedIn: false,
  userName: "Guest",
};

const views = {
  search: document.querySelector("#searchView"),
  favorite: document.querySelector("#favoriteView"),
  compare: document.querySelector("#compareView"),
  login: document.querySelector("#loginView"),
};

const pageTitle = document.querySelector("#pageTitle");
const navButtons = document.querySelectorAll("[data-view]");
const campList = document.querySelector("#campList");
const favoriteList = document.querySelector("#favoriteList");
const compareGrid = document.querySelector("#compareGrid");
const resultCount = document.querySelector("#resultCount");
const favoriteCount = document.querySelector("#favoriteCount");
const compareCount = document.querySelector("#compareCount");
const compareSummaryCount = document.querySelector("#compareSummaryCount");
const totalCampCount = document.querySelector("#totalCampCount");
const detailDialog = document.querySelector("#detailDialog");
const detailContent = document.querySelector("#detailContent");
const loginMenuText = document.querySelector("#loginMenuText");
const userChip = document.querySelector("#userChip strong");

const getCamp = (id) => camps.find((camp) => camp.id === Number(id));

function cardTemplate(camp, options = {}) {
  const isFavorite = state.favorites.has(camp.id);
  const isCompared = state.compare.has(camp.id);
  const favoriteLabel = isFavorite ? "찜 해제" : "찜하기";
  const compareLabel = isCompared ? "비교 해제" : "비교추가";
  const removeButton = options.favoriteList
    ? `<button class="icon-button remove" type="button" data-remove-favorite="${camp.id}">삭제하기</button>`
    : "";

  return `
    <article class="camp-card">
      <img src="${camp.imageUrl}" alt="${camp.name} 이미지">
      <div class="camp-info">
        <h3>${camp.name}</h3>
        <p>${camp.address}</p>
        <p>${camp.tel}</p>
        <p>${camp.facilityInfo}</p>
        <div class="camp-meta">
          <span>${camp.region}</span>
          <span>${camp.type}</span>
          <span>평점 ${camp.rating}</span>
          <span>${camp.totalSiteCount} 사이트</span>
          ${camp.petAllowed ? `<span class="tag pet">반려동물 가능</span>` : `<span>반려동물 불가</span>`}
        </div>
      </div>
      <div class="card-actions">
        <button class="icon-button detail" type="button" data-detail="${camp.id}">상세보기</button>
       <button
  class="icon-button favorite ${isFavorite ? "active" : ""}"
  type="button"
  data-favorite="${camp.id}">
  ${favoriteLabel}
</button>

<button
  class="icon-button compare ${isCompared ? "active" : ""}"
  type="button"
  data-compare="${camp.id}">
  ${compareLabel}
</button>
        ${removeButton}
      </div>
    </article>
  `;
}

function emptyTemplate(title, copy) {
  return `
    <div class="empty-state">
      <div>
        <strong>${title}</strong>
        <span>${copy}</span>
      </div>
    </div>
  `;
}

function renderSearch() {
  resultCount.textContent = state.filtered.length;
  campList.innerHTML =
    state.filtered.length > 0
      ? state.filtered.map((camp) => cardTemplate(camp)).join("")
      : emptyTemplate("검색 결과가 없습니다.", "조건을 조금 넓혀 다시 검색해보세요.");
}

function renderFavorites() {
  const favoriteCamps = camps.filter((camp) => state.favorites.has(camp.id));
  favoriteList.innerHTML =
    favoriteCamps.length > 0
      ? favoriteCamps.map((camp) => cardTemplate(camp, { favoriteList: true })).join("")
      : emptyTemplate("아직 찜한 캠핑장이 없습니다.", "검색 화면에서 찜하기를 눌러보세요.");
}

function renderCompare() {
  const compareCamps = camps.filter((camp) => state.compare.has(camp.id));
  compareSummaryCount.textContent = compareCamps.length;
  compareGrid.innerHTML =
    compareCamps.length > 0
      ? compareCamps
          .map(
            (camp) => `
              <article class="compare-card">
                <img src="${camp.imageUrl}" alt="${camp.name} 이미지">
                <div class="compare-body">
                  <h3>${camp.name}</h3>
                  <div class="compare-row"><b>주소</b><span>${camp.address}</span></div>
                  <div class="compare-row"><b>위치/거리</b><span>${camp.distanceText}</span></div>
                  <div class="compare-row"><b>반려동물</b><span>${camp.petAllowed ? "가능" : "불가능"}</span></div>
                  <div class="compare-row"><b>홈페이지</b><span>${camp.homepage}</span></div>
                  <button class="icon-button remove" type="button" data-compare="${camp.id}">비교 삭제</button>
                </div>
              </article>
            `,
          )
          .join("")
      : emptyTemplate("아직 비교할 캠핑장이 없습니다.", "검색 화면에서 비교추가를 눌러보세요.");
}

function renderCounts() {
  favoriteCount.textContent = state.favorites.size;
  compareCount.textContent = state.compare.size;
  totalCampCount.textContent = camps.length;
  loginMenuText.textContent = state.loggedIn ? "로그아웃" : "로그인";
  userChip.textContent = state.userName;
}

function renderAll() {
  renderSearch();
  renderFavorites();
  renderCompare();
  renderCounts();
}

function switchView(viewName) {
  if (viewName === "login" && state.loggedIn) {
    state.loggedIn = false;
    state.userName = "Guest";
    viewName = "search";
  }

  state.view = viewName;
  Object.entries(views).forEach(([name, element]) => element.classList.toggle("active", name === viewName));
  document.querySelectorAll(".nav button").forEach((button) => {
    button.classList.toggle("active", button.dataset.view === viewName);
  });

  const titles = {
    search: "캠핑장 검색",
    favorite: "찜 목록",
    compare: "캠핑장 비교",
    login: "로그인",
  };
  pageTitle.textContent = titles[viewName];
  renderAll();
  window.scrollTo({ top: 0, behavior: "smooth" });
}

function applySearch() {
  const keyword = document.querySelector("#keywordInput").value.trim().toLowerCase();
  const region = document.querySelector("#regionSelect").value;
  const type = document.querySelector("#typeSelect").value;
  const petOnly = document.querySelector("#petOnly").checked;

  state.filtered = camps.filter((camp) => {
    const text = `${camp.name} ${camp.address} ${camp.tel} ${camp.facilityInfo} ${camp.region} ${camp.type}`.toLowerCase();
    return (
      (!keyword || text.includes(keyword)) &&
      (!region || camp.region === region) &&
      (!type || camp.type === type) &&
      (!petOnly || camp.petAllowed)
    );
  });

  renderSearch();
}

function toggleFavorite(id) {
  const campId = Number(id);
  state.favorites.has(campId) ? state.favorites.delete(campId) : state.favorites.add(campId);
  renderAll();
}

function toggleCompare(id) {
  const campId = Number(id);
  state.compare.has(campId) ? state.compare.delete(campId) : state.compare.add(campId);
  renderAll();
}

function openDetail(id) {
  const camp = getCamp(id);
  if (!camp) return;

  detailContent.innerHTML = `
    <img src="${camp.imageUrl}" alt="${camp.name} 이미지">
    <div class="detail-content">
      <p class="eyebrow">Camp Detail</p>
      <h2>${camp.name}</h2>
      <div class="detail-row"><b>주소</b><span>${camp.address}</span></div>
      <div class="detail-row"><b>전화번호</b><span>${camp.tel}</span></div>
      <div class="detail-row"><b>시설 정보</b><span>${camp.facilityInfo}</span></div>
      <div class="detail-row"><b>홈페이지</b><a href="${camp.homepage}" target="_blank" rel="noreferrer">${camp.homepage}</a></div>
      <div class="detail-row"><b>반려동물</b><span>${camp.petAllowed ? "가능" : "불가능"}</span></div>
      <div class="detail-actions">

  <button
    class="icon-button favorite ${state.favorites.has(camp.id) ? "active" : ""}"
    type="button"
    data-favorite="${camp.id}">

    ${state.favorites.has(camp.id) ? "찜해제" : "찜하기"}

  </button>

  <button
    class="icon-button compare ${state.compare.has(camp.id) ? "active" : ""}"
    type="button"
    data-compare="${camp.id}">

    ${state.compare.has(camp.id) ? "비교해제" : "비교하기"}

  </button>

  <button
    class="soft-button"
    type="button"
    data-close-dialog>

    닫기

  </button>

</div>
    </div>
  `;

  detailDialog.showModal();
}

document.querySelector("#searchForm").addEventListener("submit", (event) => {
  event.preventDefault();
  applySearch();
});

document.querySelector("#loginForm").addEventListener("submit", (event) => {
  event.preventDefault();
  const id = document.querySelector("#loginId").value.trim();
  state.loggedIn = true;
  state.userName = id || "Camper";
  switchView("search");
});

document.body.addEventListener("click", (event) => {
  const viewButton = event.target.closest("[data-view]");
  const detailButton = event.target.closest("[data-detail]");
  const favoriteButton = event.target.closest("[data-favorite]");
  const removeFavoriteButton = event.target.closest("[data-remove-favorite]");
  const compareButton = event.target.closest("[data-compare]");
  const closeButton = event.target.closest("[data-close-dialog]");

  if (viewButton) {
    event.preventDefault();
    switchView(viewButton.dataset.view);
  }
  if (detailButton) openDetail(detailButton.dataset.detail);
  if (favoriteButton) toggleFavorite(favoriteButton.dataset.favorite);
  if (removeFavoriteButton) {
    state.favorites.delete(Number(removeFavoriteButton.dataset.removeFavorite));
    renderAll();
  }
  if (compareButton) toggleCompare(compareButton.dataset.compare);
  if (closeButton) detailDialog.close();
});

detailDialog.addEventListener("click", (event) => {
  if (event.target === detailDialog) detailDialog.close();
});

renderAll();
