const API_BASE_URL = "http://localhost:5190";
const fallbackImages = ["assets/camp-lake.png", "assets/camp-feed.png", "assets/camp-gear.png", "assets/camp-forest.png"];

let currentPage = 1;

let totalCount = 0;

let allLoadedCamps = [];

let popularCamps = [];

const pageSize = 21;

let isLoading = false;

let hasMoreData = true;

let camps = [
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
    latitude: 37.8931,
    longitude: 127.4644,
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
    latitude: 37.8315,
    longitude: 127.5106,
  },
  {
    id: 3,
    name: "태안 솔바람 오토캠프",
    address: "충남 태안군 남면 몽산포길 203",
    tel: "041-333-9012",
    region: "충남",
    type: "카라반",
    facilityInfo: "해변 5분 거리, 개수대, 장작 판매, 일몰 전망",
    imageUrl: "assets/camp-gear.png",
    homepage: "https://example.com/solbaram",
    petAllowed: true,
    totalSiteCount: 55,
    distanceText: "서울 기준 약 2시간 15분",
    rating: 4.6,
    latitude: 36.6712,
    longitude: 126.2866,
  },
  {
    id: 4,
    name: "제주 오름 백패커스팟",
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
    latitude: 33.4598,
    longitude: 126.7732,
  },
];

const state = {
  view: "search",
  filtered: [...camps],
  favorites: new Set(),
  compare: new Set(),
  checklist: {
    items: [],
    headCount: 4,
    checkedIds: new Set(),
  },
  loggedIn: false,
  userId: "",
  userName: "Guest",
  apiConnected: false,
};

const views = {
  search: document.querySelector("#searchView"),
  popular: document.querySelector("#popularView"),
  favorite: document.querySelector("#favoriteView"),
  compare: document.querySelector("#compareView"),
  map: document.querySelector("#mapView"),
  login: document.querySelector("#loginView"),
  checklist: document.querySelector("#checklistView"),
};

const pageTitle = document.querySelector("#pageTitle");
const pageEyebrow = document.querySelector("#pageEyebrow");
const campList = document.querySelector("#campList");
const pagination = document.querySelector("#pagination");
const favoriteList = document.querySelector("#favoriteList");
const compareGrid = document.querySelector("#compareGrid");
const mapResults = document.querySelector("#mapResults");
const resultCount = document.querySelector("#resultCount");
const favoriteCount = document.querySelector("#favoriteCount");
const compareCount = document.querySelector("#compareCount");
const dashboardFavoriteCount = document.querySelector("#dashboardFavoriteCount");
const dashboardCompareCount = document.querySelector("#dashboardCompareCount");
const compareSummaryCount = document.querySelector("#compareSummaryCount");
const totalCampCount = document.querySelector("#totalCampCount");
const detailDialog = document.querySelector("#detailDialog");
const detailContent = document.querySelector("#detailContent");
const loginMenuText = document.querySelector("#loginMenuText");
const userChip = document.querySelector("#userChip strong");
const mainOnlySections = document.querySelectorAll(".main-only");
const keywordInput = document.querySelector("#keywordInput");
const recentSearchBox = document.querySelector("#recentSearchBox");
const recentSearchList = document.querySelector("#recentSearchList");
const clearRecentSearches = document.querySelector("#clearRecentSearches");

function escapeHtml(value = "") {
  return String(value).replace(/[&<>'"]/g, (char) => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", "'": "&#39;", '"': "&quot;" }[char]));
}

function extractRegion(address = "") {
  const first = String(address).split(" ")[0] || "전국";
  return first.replace("특별자치도", "").replace("광역시", "").replace("특별시", "").replace("도", "");
}

function inferCampType(text = "") {
  if (text.includes("글램")) return "글램핑";
  if (text.includes("카라반")) return "카라반";
  if (text.includes("자동차") || text.includes("오토")) return "오토캠핑";
  return "일반야영장";
}

function normalizeCamp(camp, index = 0) {
  const address = camp.address || camp.addr1 || "주소 정보 없음";
  const region = camp.region || camp.doNm || extractRegion(address);
  const facilityInfo = camp.facilityInfo || camp.sbrsCl || camp.featureNm || camp.lineIntro || "시설 정보 준비 중";
  const totalSiteCount = Number(camp.totalSiteCount ?? camp.siteCount ?? camp.autoSiteCo ?? 0) || 0;

  return {
    id: Number(camp.id ?? camp.campId ?? camp.contentId ?? index + 1),
    name: camp.name || camp.facltNm || "이름 없는 캠핑장",
    address,
    tel: camp.tel || "전화번호 정보 없음",
    region,
    type: camp.type || camp.induty || inferCampType(facilityInfo),
    facilityInfo,
    imageUrl: camp.imageUrl || camp.firstImageUrl || fallbackImages[index % fallbackImages.length],
    homepage: camp.homepage || camp.resveUrl || "",
    petAllowed: Boolean(camp.petAllowed) || String(camp.animalCmgCl || "").includes("가능"),
    totalSiteCount,
    distanceText: camp.distanceText || `${region} 지역`,
    rating: Number(camp.rating || (4.4 + (index % 5) * 0.1)).toFixed(1),
    latitude: camp.latitude ?? camp.mapY,
    longitude: camp.longitude ?? camp.mapX,
    viewCount:
    Number(camp.viewCount ?? 0),
  };
}

async function saveSearchHistory(keyword) {

  if (!state.loggedIn || !state.userId)
    return;

  const cleanKeyword =
    keyword.trim();

  if (!cleanKeyword)
    return;

  try {

    await apiRequest(
      "/api/search-histories",
      {
        method: "POST",

        body: JSON.stringify({
          userId: state.userId,
          keyword: cleanKeyword,
        }),
      }
    );

  } catch (error) {

    console.warn(
      "최근 검색 기록 저장 실패",
      error
    );
  }
}

async function loadSearchHistories() {

  if (!state.loggedIn || !state.userId) {

    hideRecentSearchBox();
    return;
  }

  try {

    const histories =
      await apiRequest(
        `/api/search-histories/${state.userId}`
      );

    if (
      !Array.isArray(histories) ||
      histories.length === 0
    ) {

      recentSearchList.innerHTML = `
        <div class="recent-search-empty">
          최근 검색 기록이 없습니다.
        </div>
      `;

    } else {

      recentSearchList.innerHTML =
        histories
          .map(
            (history) => `
              <button
                class="recent-search-keyword"
                type="button"
                data-recent-keyword="${history.keyword}">
                ${history.keyword}
              </button>
            `
          )
          .join("");
    }

    recentSearchBox.classList.remove(
      "hidden"
    );

  } catch (error) {

    console.warn(
      "최근 검색 기록 조회 실패",
      error
    );
  }
}

async function clearSearchHistories() {

  if (!state.loggedIn || !state.userId)
    return;

  try {

    await apiRequest(
      `/api/search-histories/${state.userId}`,
      {
        method: "DELETE",
      }
    );

    await loadSearchHistories();

  } catch (error) {

    console.warn(
      "최근 검색 기록 삭제 실패",
      error
    );
  }
}

function hideRecentSearchBox() {

  if (recentSearchBox) {

    recentSearchBox.classList.add(
      "hidden"
    );
  }
}

async function apiRequest(path, options = {}) {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      ...(options.body ? { "Content-Type": "application/json" } : {}),
      ...(options.headers || {}),
    },
    ...options,
  });

  if (!response.ok) {
    throw new Error((await response.text()) || `API request failed: ${response.status}`);
  }

  const text = await response.text();
  return text ? JSON.parse(text) : null;
}

async function loadCampsFromServer(page = 1) {

  if (isLoading || !hasMoreData)
    return;

  isLoading = true;

  try {

    const response =
      await apiRequest(
        `/api/camps?pageNo=${page}&numOfRows=${pageSize}`
      );

    const data = response.items;

    totalCount =
      response.totalCount;

    const normalized =
      data.map((camp, index) =>
        normalizeCamp(camp, index)
      );

    if (normalized.length === 0) {

      hasMoreData = false;
      return;
    }

    camps = [...normalized];

    allLoadedCamps = [
      ...allLoadedCamps,
      ...normalized
    ];

    state.filtered = [...camps];

    state.apiConnected = true;

    renderSearch(camps);

    renderPagination(totalCount);

    console.log(
      "서버 데이터 추가 로드 성공",
      normalized.length
    );

  } catch (error) {

    state.apiConnected = false;

    console.error(
      "서버 연결 실패",
      error
    );

  } finally {

    isLoading = false;
  }
}

function getCamp(id) {

  return allLoadedCamps.find(
    (camp) => camp.id === Number(id)
  );
}

function cardTemplate(camp, options = {}) {
  const isFavorite = state.favorites.has(camp.id);
  const isCompared = state.compare.has(camp.id);
  const removeButton = options.favoriteList
    ? ``
    : "";

  return `
    <article class="camp-card">
      <img src="${escapeHtml(camp.imageUrl)}" alt="${escapeHtml(camp.name)} 이미지" onerror="this.src='assets/camp-lake.png'">
      <div class="camp-info">
        <h3>${escapeHtml(camp.name)}</h3>
        <p>${escapeHtml(camp.address)}</p>
        <p>${escapeHtml(camp.tel)}</p>
        <p>${escapeHtml(camp.facilityInfo)}</p>
        <div class="camp-meta">
          <span>${escapeHtml(camp.region)}</span>
          <span>${escapeHtml(camp.type)}</span>
          <span>${camp.petAllowed ? "반려동물 가능" : "반려동물 불가"}</span>
        </div>
      </div>
      <div class="view-count">
        👁 ${camp.viewCount}
      </div>
      <div class="card-actions">
        <button class="icon-button detail" type="button" data-detail="${camp.id}">상세보기</button>
        <button class="icon-button favorite ${isFavorite ? "active" : ""}" type="button" data-favorite="${camp.id}">${isFavorite ? "찜 해제" : "찜하기"}</button>
        <button class="icon-button compare ${isCompared ? "active" : ""}" type="button" data-compare="${camp.id}">${isCompared ? "비교 해제" : "비교 추가"}</button>
        ${removeButton}
      </div>
    </article>`;
}

function emptyTemplate(title, copy) {
  return `<div class="empty-state"><div><strong>${escapeHtml(title)}</strong><span>${escapeHtml(copy)}</span></div></div>`;
}

function renderSearch(list = camps) {

  const targetList =

    list && list.length > 0

      ? list

      : camps;

  resultCount.textContent =
    totalCount;

  campList.innerHTML =

    targetList.length

      ? targetList
          .map((camp) =>
            cardTemplate(camp)
          )
          .join("")

      : emptyTemplate(
          "검색 결과가 없습니다.",
          "조건을 조금 넓혀 다시 검색해보세요."
        );
}

function renderFavorites() {
  const favoriteCamps = allLoadedCamps.filter((camp) => state.favorites.has(camp.id));
  favoriteList.innerHTML = favoriteCamps.length
    ? favoriteCamps.map((camp) => cardTemplate(camp, { favoriteList: true })).join("")
    : emptyTemplate("아직 즐겨찾기가 없습니다.", "검색 화면에서 찜하기를 눌러보세요.");
}

function renderCompare() {
  const compareCamps = allLoadedCamps.filter((camp) => state.compare.has(camp.id));
  compareSummaryCount.textContent = compareCamps.length;
  compareGrid.innerHTML = compareCamps.length
    ? compareCamps.map((camp) => `
      <article class="compare-card">
        <img src="${escapeHtml(camp.imageUrl)}" alt="${escapeHtml(camp.name)} 이미지" onerror="this.src='assets/camp-lake.png'">
        <div class="compare-body">
          <h3>${escapeHtml(camp.name)}</h3>
          <div class="compare-row"><b>주소</b><span>${escapeHtml(camp.address)}</span></div>
          <div class="compare-row"><b>위치/거리</b><span>${escapeHtml(camp.distanceText)}</span></div>
          <div class="compare-row"><b>반려동물</b><span>${camp.petAllowed ? "가능" : "불가"}</span></div>
          <div class="compare-row"><b>홈페이지</b><span>${escapeHtml(camp.homepage || "정보 없음")}</span></div>
          <button class="icon-button remove" type="button" data-compare="${camp.id}">비교 제거</button>
        </div>
      </article>`).join("")
    : emptyTemplate("아직 비교할 캠핑장이 없습니다.", "검색 화면에서 비교 추가를 눌러보세요.");
}

// 🆕 체크리스트 로딩 함수
async function loadChecklist(headCount) {
  try {
    if (!state.apiConnected) {
      alert("서버에 연결할 수 없습니다. 나중에 다시 시도해주세요.");
      return;
    }

    const result = await apiRequest(`/api/CampItem/recommend?headCount=${headCount}`);
    if (!Array.isArray(result)) {
      throw new Error("Invalid response format");
    }

    // 기존 체크된 항목 유지
    state.checklist.items = result.map((item) => ({
      ...item,
      isChecked: state.checklist.checkedIds.has(item.id) || false,
    }));
    state.checklist.headCount = headCount;

    renderChecklist();
    document.getElementById("checklistContainer").style.display = "block";
    document.getElementById("checklistEmpty").style.display = "none";
  } catch (error) {
    console.error("체크리스트 로딩 실패:", error);
    alert("물품 목록을 불러올 수 없습니다. 나중에 다시 시도해주세요.");
  }
}

// 🆕 체크리스트 렌더링 함수
function renderChecklist() {
  const container = document.getElementById("checklistCategories");
  const items = state.checklist.items;

  if (!items || items.length === 0) {
    document.getElementById("checklistContainer").style.display = "none";
    document.getElementById("checklistEmpty").style.display = "block";
    return;
  }

  // 카테고리별 분류
  const categorized = {};
  items.forEach((item) => {
    if (!categorized[item.category]) {
      categorized[item.category] = [];
    }
    categorized[item.category].push(item);
  });

  let html = "";
  Object.entries(categorized).forEach(([category, categoryItems]) => {
    html += `
      <div class="checklist-category">
        <div class="category-header">
          <h3>${escapeHtml(category)}</h3>
          <span class="category-count">${categoryItems.filter((i) => i.isChecked).length}/${categoryItems.length}</span>
        </div>
        <div class="category-items">
    `;

    categoryItems.forEach((item) => {
      const isChecked = state.checklist.checkedIds.has(item.id);
      html += `
        <div class="checklist-item ${isChecked ? "checked" : ""}">
          <input 
            type="checkbox" 
            id="item-${item.id}" 
            ${isChecked ? "checked" : ""} 
            data-item-id="${item.id}"
            class="checklist-checkbox"
          />
          <label for="item-${item.id}" class="item-label">
            <div class="item-info">
              <span class="item-name">${escapeHtml(item.itemName)}</span>
              <span class="item-quantity">${item.recommendedQuantity}${escapeHtml(item.unit)}</span>
            </div>
          </label>
        </div>
      `;
    });

    html += `</div></div>`;
  });

  container.innerHTML = html;

  // 통계 업데이트
  const totalItems = items.length;
  const checkedItems = Array.from(state.checklist.checkedIds).filter((id) => items.some((item) => item.id === id)).length;
  document.getElementById("totalItems").textContent = totalItems;
  document.getElementById("checkedItems").textContent = checkedItems;

  // 체크박스 이벤트 리스너 추가
  container.querySelectorAll(".checklist-checkbox").forEach((checkbox) => {
    checkbox.addEventListener("change", (e) => {
      const itemId = Number(e.target.dataset.itemId);
      if (e.target.checked) {
        state.checklist.checkedIds.add(itemId);
      } else {
        state.checklist.checkedIds.delete(itemId);
      }
      const category =
        checkbox.closest(
          ".checklist-category"
        );

      const checkedCount =

        category.querySelectorAll(
          ".checklist-checkbox:checked"
        ).length;

      const totalCount =

        category.querySelectorAll(
          ".checklist-checkbox"
        ).length;

      category.querySelector(
        ".category-count"
      ).textContent =
        `${checkedCount}/${totalCount}`;
      updateChecklistStats();
      e.target.closest(".checklist-item").classList.toggle("checked");
    });
  });
}

// 🆕 체크리스트 통계 업데이트
function updateChecklistStats() {
  const items = state.checklist.items;
  const checkedItems = Array.from(state.checklist.checkedIds).filter((id) => items.some((item) => item.id === id)).length;
  document.getElementById("checkedItems").textContent = checkedItems;
}

// 🆕 전부 체크
function checkAllItems() {
  state.checklist.items.forEach((item) => {
    state.checklist.checkedIds.add(item.id);
  });
  document.querySelectorAll(".checklist-checkbox").forEach((checkbox) => {
    checkbox.checked = true;
    checkbox.closest(".checklist-item").classList.add("checked");
  });
  updateChecklistStats();
}

// 🆕 전부 해제
function uncheckAllItems() {
  state.checklist.checkedIds.clear();
  document.querySelectorAll(".checklist-checkbox").forEach((checkbox) => {
    checkbox.checked = false;
    checkbox.closest(".checklist-item").classList.remove("checked");
  });
  updateChecklistStats();
}

// 🆕 체크리스트 내보내기
function exportChecklist() {
  const headCount = state.checklist.headCount;
  const items = state.checklist.items;
  const checkedItems = items.filter((item) => state.checklist.checkedIds.has(item.id));

  let text = `🏕️ 캠핑 물품 체크리스트\n`;
  text += `인원수: ${headCount}명\n`;
  text += `생성일시: ${new Date().toLocaleString("ko-KR")}\n\n`;
  text += `체크된 항목 (${checkedItems.length}/${items.length})\n`;
  text += `════════════════════════════════════\n\n`;

  let currentCategory = "";
  checkedItems.forEach((item) => {
    if (item.category !== currentCategory) {
      currentCategory = item.category;
      text += `\n[${currentCategory}]\n`;
    }
    text += `✓ ${item.itemName} - ${item.recommendedQuantity}${item.unit}\n`;
  });

  text += `\n\n미체크된 항목 (${items.length - checkedItems.length}/${items.length})\n`;
  text += `════════════════════════════════════\n\n`;

  currentCategory = "";
  const uncheckedItems = items.filter((item) => !state.checklist.checkedIds.has(item.id));
  uncheckedItems.forEach((item) => {
    if (item.category !== currentCategory) {
      currentCategory = item.category;
      text += `\n[${currentCategory}]\n`;
    }
    text += `☐ ${item.itemName} - ${item.recommendedQuantity}${item.unit}\n`;
  });

  navigator.clipboard.writeText(text).then(() => {
    alert("물품 목록이 클립보드에 복사되었습니다!");
  });
}

// 🆕 체크리스트 인쇄
function printChecklist() {
  const printWindow = window.open("", "", "width=800,height=600");
  const items = state.checklist.items;

  let html = `
    <html>
      <head>
        <title>캠핑 물품 체크리스트</title>
        <style>
          body { font-family: Arial, sans-serif; margin: 20px; }
          h1 { text-align: center; }
          .meta { text-align: center; color: #666; margin: 10px 0 20px; }
          .category { margin: 20px 0; }
          .category h3 { border-bottom: 2px solid #333; padding: 10px 0; }
          .item { display: flex; padding: 5px 0; }
          .checkbox { width: 20px; }
          .item-name { flex: 1; }
          .item-qty { width: 100px; }
        </style>
      </head>
      <body>
        <h1>🏕️ 캠핑 물품 체크리스트</h1>
        <div class="meta">
          <p>인원수: ${state.checklist.headCount}명 | 생성일: ${new Date().toLocaleDateString("ko-KR")}</p>
        </div>
  `;

  const categorized = {};
  items.forEach((item) => {
    if (!categorized[item.category]) {
      categorized[item.category] = [];
    }
    categorized[item.category].push(item);
  });

  Object.entries(categorized).forEach(([category, categoryItems]) => {
    html += `<div class="category"><h3>${category}</h3>`;
    categoryItems.forEach((item) => {
      const isChecked = state.checklist.checkedIds.has(item.id);
      html += `
        <div class="item">
          <div class="checkbox">${isChecked ? "☑️" : "☐"}</div>
          <div class="item-name">${item.itemName}</div>
          <div class="item-qty">${item.recommendedQuantity}${item.unit}</div>
        </div>
      `;
    });
    html += `</div>`;
  });

  html += `</body></html>`;
  printWindow.document.write(html);
  printWindow.document.close();
  printWindow.print();
}

function renderMap() {
  mapResults.innerHTML = camps.map((camp) => `
    <article class="map-result-card">
      <img src="${escapeHtml(camp.imageUrl)}" alt="${escapeHtml(camp.name)} 이미지" onerror="this.src='assets/camp-lake.png'">
      <div><strong>${escapeHtml(camp.name)}</strong><span>${escapeHtml(camp.address)}</span><small>${escapeHtml(camp.distanceText)}</small></div>
      <button class="soft-button" type="button" data-detail="${camp.id}">보기</button>
    </article>`).join("");
}

function renderCounts() {
  favoriteCount.textContent = state.favorites.size;
  compareCount.textContent = state.compare.size;
  if (dashboardFavoriteCount) dashboardFavoriteCount.textContent = state.favorites.size;
  if (dashboardCompareCount) dashboardCompareCount.textContent = state.compare.size;
  if (totalCampCount) totalCampCount.textContent = totalCount;
  if (loginMenuText) loginMenuText.textContent = state.loggedIn ? "로그아웃" : "로그인";
  if (userChip) userChip.textContent = state.userName;
}

function renderAll() {
  renderSearch();
  renderFavorites();
  renderCompare();
  renderMap();
  renderCounts();
  renderChecklist();
}

function switchView(viewName) {
  if (viewName === "login" && state.loggedIn) {
    state.loggedIn = false;
    state.userId = "";
    state.userName = "Guest";
    state.favorites.clear();
    localStorage.removeItem("campingUser");
    renderAll();
    viewName = "search";
  }

  state.view = viewName;
  mainOnlySections.forEach((section) => section.classList.toggle("hidden", viewName !== "search"));
  Object.entries(views).forEach(([name, element]) => element?.classList.toggle("active", name === viewName));
  document.querySelectorAll("[data-view]").forEach((button) => button.classList.toggle("active", button.dataset.view === viewName));

   const titles = { search: "🏕️ 캠핑장 검색", popular: "🔥 인기 캠핑장 TOP 100", favorite: "⭐️ 즐겨찾기", compare: "⚖️ 캠핑장 비교", checklist: "📦 물품 체크리스트", map: "🗺️ 지도", login: "🔐 로그인" };
  const eyebrows = { search: "Camping Finder", favorite: "Saved Camps", compare: "Compare Camps", checklist: "Packing Checklist", map: "Map Search", login: "Welcome Back" };
  if (pageTitle) pageTitle.textContent = titles[viewName] || titles.search;
  if (pageEyebrow) pageEyebrow.textContent = eyebrows[viewName] || eyebrows.search;

  renderAll();
  if (viewName === "map") renderMainMap();
  window.scrollTo({ top: 0, behavior: "smooth" });
}

async function applySearch(page = 1) {

  const originalKeyword =
    keywordInput.value.trim();

  const keyword =
    originalKeyword.toLowerCase();

  const region =
    document
      .querySelector("#regionSelect")
      .value;

  const petOnly =
    document
      .querySelector("#petOnly")
      .checked;

  await saveSearchHistory(
    originalKeyword
  );

  hideRecentSearchBox();

  if (
    !keyword &&
    !region &&
    !petOnly
  ) {

    await loadCampsFromServer(1);

    return;
  }

  try {

    let query =
      `/api/camps/search?keyword=${encodeURIComponent(keyword)}&pageNo=${page}&numOfRows=${pageSize}`;

    if (region) {

      query +=
        `&region=${encodeURIComponent(region)}`;
    }

    if (petOnly) {

      query +=
        `&petOnly=true`;
    }

    const response =
      await apiRequest(query);

    totalCount =
      response.totalCount;

    state.filtered =
      response.items.map(
        (camp, index) =>
          normalizeCamp(
            camp,
            index
          )
      );

    renderSearch(
      state.filtered
    );

    renderPagination(
      totalCount
    );

  } catch (error) {

    console.error(
      "검색 실패",
      error
    );
  }
}

async function toggleFavorite(id) {
  const campId = Number(id);
  const shouldAdd = !state.favorites.has(campId);

  if (shouldAdd) {
    state.favorites.add(campId);
    if (state.apiConnected && state.loggedIn && state.userId) {
      try {
        await apiRequest("/api/favorites", { method: "POST", body: JSON.stringify({ userId: state.userId, campId }) });
      } catch (error) {
        console.warn("서버 즐겨찾기 추가 실패", error);
      }
    }
  } else {
    state.favorites.delete(campId);
    if (state.apiConnected && state.loggedIn && state.userId) {
      try {
        await apiRequest(`/api/favorites?userId=${encodeURIComponent(state.userId)}&campId=${campId}`, { method: "DELETE" });
      } catch (error) {
        console.warn("서버 즐겨찾기 삭제 실패", error);
      }
    }
  }

  renderAll();
  if (detailDialog.open) openDetail(campId);
}

async function loadFavorites() {
  if (!state.apiConnected || !state.loggedIn || !state.userId) return;

  try {
    const favorites = await apiRequest(`/api/favorites/${encodeURIComponent(state.userId)}`);
    state.favorites = new Set((favorites || []).map((item) => Number(item.campId ?? item.camp?.id)).filter(Boolean));
  } catch (error) {
    console.warn("즐겨찾기 불러오기 실패", error);
  }
}

async function loadPopularCamps() {

  try {

    const data =
      await apiRequest(
        "/api/camps/popular"
      );

    popularCamps =

      data.map((camp, index) =>

        normalizeCamp(
          camp,
          index
        )
      );

    renderPopularCamps();

  } catch (error) {

    console.error(
      "인기 캠핑장 로드 실패",
      error
    );
  }
}

function renderPopularCamps() {

  const container =
    document.querySelector(
      "#popularCampGrid"
    );

  if (!container)
    return;

  container.innerHTML =

    popularCamps.map((camp) =>

      cardTemplate(camp)

    ).join("");
}

function toggleCompare(id) {
  const campId = Number(id);
  if (state.compare.has(campId)) state.compare.delete(campId);
  else state.compare.add(campId);

  renderAll();
  if (detailDialog.open) openDetail(campId);
}

async function openDetail(id) {

  const response =
    await apiRequest(`/api/camps/${id}`);

  const camp =
    normalizeCamp(response);

  if (!camp) return;

  detailContent.innerHTML = `
    <img src="${escapeHtml(camp.imageUrl)}" alt="${escapeHtml(camp.name)} 이미지" onerror="this.src='assets/camp-lake.png'">
    <div class="detail-content">
      <p class="eyebrow">Camp Detail</p>
      <h2>${escapeHtml(camp.name)}</h2>
      <div class="detail-row"><b>주소</b><span>${escapeHtml(camp.address)}</span></div>
      <div class="detail-row"><b>전화번호</b><span>${escapeHtml(camp.tel)}</span></div>
      <div class="detail-row"><b>시설 정보</b><span>${escapeHtml(camp.facilityInfo)}</span></div>
      <div class="detail-row"><b>홈페이지</b><a href="${escapeHtml(camp.homepage)}" target="_blank">${escapeHtml(camp.homepage || "정보 없음")}</a></div>
      <div class="detail-row"><b>반려동물</b><span>${camp.petAllowed ? "가능" : "불가"}</span></div>
      <div class="detail-map-title">위치 지도</div>
      <div id="detailMap" class="detail-map"></div>
      <div class="detail-actions">
        <button class="icon-button favorite ${state.favorites.has(camp.id) ? "active" : ""}" type="button" data-favorite="${camp.id}">${state.favorites.has(camp.id) ? "찜 해제" : "찜하기"}</button>
        <button class="icon-button compare ${state.compare.has(camp.id) ? "active" : ""}" type="button" data-compare="${camp.id}">${state.compare.has(camp.id) ? "비교 해제" : "비교하기"}</button>
        <button class="soft-button" type="button" data-close-dialog>닫기</button>
      </div>
    </div>`;

  detailDialog.showModal();
  requestAnimationFrame(() => renderKakaoMap(camp));
}

function renderKakaoMap(camp) {
  const mapContainer = document.getElementById("detailMap");
  if (!mapContainer) return;

  const latitude = Number(camp.latitude);
  const longitude = Number(camp.longitude);
  if (!latitude || !longitude) {
    mapContainer.innerHTML = '<div class="detail-map-empty">위치 정보가 없습니다.</div>';
    return;
  }

  if (!window.kakao || !window.kakao.maps) {
    mapContainer.innerHTML = '<div class="detail-map-empty">지도 SDK를 불러오지 못했습니다.</div>';
    return;
  }

  kakao.maps.load(() => {
    const position = new kakao.maps.LatLng(latitude, longitude);
    const map = new kakao.maps.Map(mapContainer, { center: position, level: 4 });
    new kakao.maps.Marker({ map, position });
    setTimeout(() => map.relayout(), 100);
  });
}

function renderMainMap() {
  const mapContainer = document.getElementById("kakaoMap");
  if (!mapContainer) return;

  if (!window.kakao || !window.kakao.maps) {
    mapContainer.innerHTML = '<div class="detail-map-empty">지도 SDK를 불러오지 못했습니다.</div>';
    return;
  }

  kakao.maps.load(() => {
    const map = new kakao.maps.Map(mapContainer, { center: new kakao.maps.LatLng(36.5, 127.8), level: 13 });
    camps.forEach((camp) => {
      const lat = Number(camp.latitude);
      const lng = Number(camp.longitude);
      if (!lat || !lng) return;
      const marker = new kakao.maps.Marker({ map, position: new kakao.maps.LatLng(lat, lng) });
      kakao.maps.event.addListener(marker, "click", () => openDetail(camp.id));
    });
  });
}

document.querySelector("#searchForm")?.addEventListener("submit", (event) => {
  event.preventDefault();
  applySearch();
});

document.querySelector("#loginForm")?.addEventListener("submit", async (event) => {
  event.preventDefault();
  const id = document.querySelector("#loginId").value.trim();
  const password = document.querySelector("#loginPw").value;

  if (state.apiConnected && id) {
    try {
      const user = await apiRequest("/api/users/login", { method: "POST", body: JSON.stringify({ userId: id, password }) });
      state.loggedIn = true;
      state.userId = user.userId || id;
      state.userName = user.userName || user.userId || id;
      localStorage.setItem("campingUser", JSON.stringify({ loggedIn: true, userId: state.userId, userName: state.userName }));
      await loadFavorites();
    } catch (error) {
      alert("로그인에 실패했습니다. 아이디와 비밀번호를 확인해주세요.");
      return;
    }
  } else {

    state.loggedIn = true;

    state.userId =
      id || "demo";

    state.userName =
      id || "Camper";

    localStorage.setItem(
      "campingUser",
      JSON.stringify({
        loggedIn: true,
        userId: state.userId,
        userName: state.userName,
      })
    );
  }

  switchView("search");
});

keywordInput?.addEventListener(
  "focus",
  async () => {

    if (state.loggedIn) {

      await loadSearchHistories();
    }
  }
);

keywordInput?.addEventListener(
  "click",
  async () => {

    if (state.loggedIn) {

      await loadSearchHistories();
    }
  }
);

clearRecentSearches?.addEventListener(
  "click",
  async (event) => {

    event.preventDefault();

    event.stopPropagation();

    await clearSearchHistories();
  }
);

document.body.addEventListener("click", (event) => {
  const viewButton = event.target.closest("[data-view]");
  const detailButton = event.target.closest("[data-detail]");
  const favoriteButton = event.target.closest("[data-favorite]");
  const removeFavoriteButton = event.target.closest("[data-remove-favorite]");
  const compareButton = event.target.closest("[data-compare]");
  const closeButton = event.target.closest("[data-close-dialog]");
  const recentKeywordButton = event.target.closest("[data-recent-keyword]");

  if (viewButton) {
    event.preventDefault();
    switchView(viewButton.dataset.view);
  }
  if (detailButton) openDetail(detailButton.dataset.detail);
  if (favoriteButton) toggleFavorite(favoriteButton.dataset.favorite);
  if (removeFavoriteButton) toggleFavorite(removeFavoriteButton.dataset.removeFavorite);
  if (compareButton) toggleCompare(compareButton.dataset.compare);
  if (closeButton) detailDialog.close();
  if (recentKeywordButton) {

  const keyword =
      recentKeywordButton
        .dataset
        .recentKeyword;

    keywordInput.value =
      keyword;

    hideRecentSearchBox();

    applySearch();

    return;
  }

  if (
    !event.target.closest(".keyword-field") &&
    !event.target.closest("#recentSearchBox")
  ) {

  hideRecentSearchBox();
  }

});

document.getElementById("incrementBtn")
  ?.addEventListener(
    "click",
    () => {

      const input =
        document.getElementById(
          "headcountInput"
        );

      const current =
        Number(input.value) || 1;

      input.value =
        Math.min(50, current + 1);
    }
  );

document.getElementById("decrementBtn")
  ?.addEventListener(
    "click",
    () => {

      const input =
        document.getElementById(
          "headcountInput"
        );

      const current =
        Number(input.value) || 1;

      input.value =
        Math.max(1, current - 1);
    }
  );

document.getElementById("loadChecklistBtn")
  ?.addEventListener(
    "click",
    () => {

      const headcount =
        Number(
          document.getElementById(
            "headcountInput"
          ).value
        ) || 4;

      loadChecklist(headcount);
    }
  );

document.getElementById("exportBtn")
  ?.addEventListener(
    "click",
    exportChecklist
  );

document.getElementById("printBtn")
  ?.addEventListener(
    "click",
    printChecklist
  );

detailDialog?.addEventListener("click", (event) => {
  if (event.target === detailDialog) detailDialog.close();
});




document.getElementById("loadChecklistBtn")?.addEventListener(
  "click",
  () => {

    const headcount =
      Number(
        document.getElementById(
          "headcountInput"
        ).value
      ) || 4;

    if (
      headcount < 1 ||
      headcount > 50
    ) {

      alert(
        "인원수는 1명 이상 50명 이하로 입력해주세요"
      );

      return;
    }

    loadChecklist(headcount);
  }
);

window.addEventListener("DOMContentLoaded", async () => {
  const savedUser = localStorage.getItem("campingUser");
  if (savedUser) {
    try {
      const userData = JSON.parse(savedUser);
      state.loggedIn = Boolean(userData.loggedIn);
      state.userId = userData.userId || "";
      state.userName = userData.userName || userData.userId || "Guest";
    } catch {
      localStorage.removeItem("campingUser");
    }
  }

  await loadCampsFromServer(currentPage);
  await loadPopularCamps();
  await loadFavorites();
  renderAll();
});

function renderPagination(totalCount) {

  const totalPages =
    Math.ceil(totalCount / pageSize);

  pagination.innerHTML = "";

  // 이전 버튼

  const prevButton =
    document.createElement("button");

  prevButton.innerHTML = "&lt;";

  prevButton.disabled =
    currentPage === 1;

  prevButton.addEventListener(
    "click",
    async () => {

      if (currentPage > 1) {

        currentPage--;

        await loadCampsFromServer(
          currentPage
        );
      }
    }
  );

  pagination.appendChild(prevButton);

  // 페이지 범위 계산

  const startPage =
    Math.max(1, currentPage - 2);

  const endPage =
    Math.min(
      totalPages,
      currentPage + 2
    );

  // 페이지 버튼

  for (
    let i = startPage;
    i <= endPage;
    i++
  ) {

    const button =
      document.createElement("button");

    button.textContent = i;

    if (i === currentPage) {
      button.classList.add("active");
    }

    button.addEventListener(
      "click",
      async () => {

        currentPage = i;

        await loadCampsFromServer(i);
      }
    );

    pagination.appendChild(button);
  }

  // 다음 버튼

  const nextButton =
    document.createElement("button");

  nextButton.innerHTML = "&gt;";

  nextButton.disabled =
    currentPage === totalPages;

  nextButton.addEventListener(
    "click",
    async () => {

      if (currentPage < totalPages) {

        currentPage++;

        await loadCampsFromServer(
          currentPage
        );
      }
    }
  );

  pagination.appendChild(nextButton);
}