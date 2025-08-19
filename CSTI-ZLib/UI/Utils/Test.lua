_tmp_window = ZGameUI.CreateWindow("Test", "Wind&Leaves_BlueberryPlant", 0, 100, 600, 600)

_tmp_window:AddOnCardDropOn(function(cards)
    for _, card in ipairs(cards) do
        card:Remove(false)
    end
end)

_tmp_window:Open()